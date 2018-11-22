using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lucky
{
    public interface MessageFilterBase
    {
        string GetTypeString();
        void FireEvent();
        void SetFilterLock(bool islock);
        bool GetFilterLock();
    }

    public class MessageFilter<T> : MessageFilterBase
    {
        static MessageFilter<T> instance;

        public static MessageFilter<T> getInstance() { if (instance == null) instance = new MessageFilter<T>(); return instance; }

        public delegate bool MessageBusHanlder(T msg);

        private bool isLocked = false;

        public void SetFilterLock(bool islock)
        {
            isLocked = islock;
        }

        public bool GetFilterLock()
        {
            return isLocked;
        }

        public List<MessageBusHanlder> hanlderList = new List<MessageBusHanlder>();
        public List<T> msgList = new List<T>();

        public bool AddListener(MessageBusHanlder fun)
        {
            if (hanlderList.Contains(fun))
                return false;
            hanlderList.Add(fun);
            return true;
        }

        public void RemoveListener(MessageBusHanlder fun)
        {
            hanlderList.Remove(fun);
        }

        public void FireEvent()
        {
            if (msgList.Count == 0)
                return;

            T tempListItem = msgList[0];
            msgList.Remove(tempListItem);
            UnityEngine.Profiling.Profiler.BeginSample("jony.msgbus,FireEvent");
            for (int i = 0; i < hanlderList.Count; i++)
            {
                UnityEngine.Profiling.Profiler.BeginSample("jony.msgbus,FireEvent," + hanlderList[i].Method.Name + ',' + tempListItem.GetType().ToString());

                (hanlderList[i])(tempListItem);
                UnityEngine.Profiling.Profiler.EndSample();

            }
            UnityEngine.Profiling.Profiler.EndSample();
            return;
        }

        public string GetTypeString()
        {
            return typeof(T).ToString();
        }

        public int GetFilterListenerNum()
        {
            return hanlderList.Count;
        }
    }

    public class MessageBus
    {
        public MessageBus()
        {
            m_fDefaultDeltaTime = Time.deltaTime;
        }

        // 消息监听列表
        public static List<MessageFilterBase> filterList = new List<MessageFilterBase>();

        private static bool isLocked = false;
        //private static float m_lockDispatcherSecond = 0.0f;
        private static float m_fDefaultDeltaTime = 0.02f;
        /// <summary>
        /// 注册监听器
        /// 同一个对象的生命周期中如果调用了该函数，需要在销毁时调用UnRegister函数以注销监听器，防止重复的调用
        /// </summary>
        /// <typeparam name="T">filter 类型</typeparam>
        /// <param name="hanlder">注册的函数</param>
        public static void Register<T>(MessageFilter<T>.MessageBusHanlder hanlder = null)
        {
            MessageFilter<T> msgFilter = MessageFilter<T>.getInstance();

            if (hanlder == null)//说明来自preinit，不是真正注册消息
            {
                return;
            }

            if (msgFilter.hanlderList.Contains(hanlder))
                return;

            msgFilter.hanlderList.Add(hanlder);
        }
        

        public static void AttachEvent<T>(MessageFilter<T>.MessageBusHanlder hanlder, bool isAttach)
        {
            if (isAttach)
            {
                Register<T>(hanlder);
            }
            else
            {
                UnRegister<T>(hanlder);
            }
        }


        /// <summary>
        ///  反注册，取消监听时调用
        /// </summary>
        /// <typeparam name="T">filter类型</typeparam>
        /// <param name="hanlder">取消注册的hanlder</param>
        public static void UnRegister<T>(MessageFilter<T>.MessageBusHanlder hanlder)
        {
            MessageFilter<T> msgFilter = MessageFilter<T>.getInstance();

            if (msgFilter != null && msgFilter.hanlderList.Count > 0)
            {
                for (int i = 0; i < msgFilter.hanlderList.Count; ++i)
                {
                    if (msgFilter.hanlderList.Contains(hanlder))
                        msgFilter.hanlderList.Remove(hanlder);
                    else
                        return;
                }
            }
        }

        /// <summary>
        ///  加入消息队列，每帧执行固定数量的消息，post后下一帧执行
        /// </summary>
        /// <typeparam name="T">调用时不用填写，会根据参数类型识别</typeparam>
        /// <param name="msg">发送的消息</param>
        public static void Post<T>(T msg)
        {
            MessageFilter<T> msgFilter = MessageFilter<T>.getInstance();
            msgFilter.msgList.Add(msg);
            filterList.Add(msgFilter);
        }

        /// <summary>
        /// 执行消息，会立刻调用对应filter的函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        public static void Send<T>(T msg)
        {
            if (isLocked/* GameSystem.instance.connection.IsLockEvent()*/)
            {
                MessageBus.Post<T>(msg);
            }
            else
            {
                MessageFilter<T> msgFilter = MessageFilter<T>.getInstance();
                if (msgFilter != null && msgFilter.hanlderList.Count != 0)
                {
                    for (int i = 0; i < msgFilter.hanlderList.Count; i++)
                    {
                        (msgFilter.hanlderList[i])(msg);
                    }
                }
            }
        }

        /// <summary>
        ///  添加消息锁
        /// </summary>
        /// <typeparam name="T">需要加锁的消息类型</typeparam>
        public static void LockFilterEvent<T>()
        {
            MessageFilter<T> msgFilter = MessageFilter<T>.getInstance();
            msgFilter.SetFilterLock(true);
        }

        /// <summary>
        /// 解除消息锁
        /// </summary>
        /// <typeparam name="T">需要解锁的消息类型</typeparam>
        public static void UnLockFilterEvent<T>()
        {
            MessageFilter<T> msgFilter = MessageFilter<T>.getInstance();
            msgFilter.SetFilterLock(false);
        }

        /// <summary>
        /// 每帧调用的执行，处理消息队列
        /// </summary>
        /// <param name="msgNum">每帧执行消息的最高数量</param>
        public static void Update(int msgNum)
        {
            if (msgNum <= 0)
                return;

            //Profiler.BeginSample("jony: messageBus.Update, CheckLock");
            //CheckLock();
            //Profiler.EndSample();

            UnityEngine.Profiling.Profiler.BeginSample("jony: messageBus.Update, GetFilterLock, FireEvent, Remove");

            while (filterList.Count > 0 && !isLocked /*!GameSystem.instance.connection.IsLockEvent()*/)//同步网络的锁，在网络消息锁住后messagebus的锁也会锁住
            {
                MessageFilterBase filterbase = filterList[0];
                if (filterbase.GetFilterLock())
                    continue;
                //Profiler.BeginSample("jony: messageBus.Update, FireEvent:" + filterbase.GetTypeString());
                filterbase.FireEvent();
                //Profiler.EndSample();


                filterList.Remove(filterList[0]);
            }
            UnityEngine.Profiling.Profiler.EndSample();


        }
        //Update end

        /// <summary>
        /// 总消息锁
        /// </summary>
        /// <param name="nSecond">锁定时间</param>
        //public static void LockEvent(float nSecond)
        //{
        //    m_lockDispatcherSecond += nSecond;
        //}
        public static void LockEvent( )
        {
            isLocked = true;
        }

        /// <summary>
        /// 解锁消息
        /// </summary>
        public static void UnLockEvent()
        {
            //m_lockDispatcherSecond = 0.0f;
            isLocked = false;
        }

        /// <summary>
        /// 判断消息锁是否开启，暂时未使用
        /// </summary>
        /// <returns></returns>
        static bool IsLockEvent()
        {
            return isLocked;// m_lockDispatcherSecond > 0.0f;
        }

        public static void LockFromNetwork()
        {
            isLocked = true;
        }
        public static void UnLockFromNetwork()
        {
            isLocked = false;
        }
    }
}
