using UnityEngine;
using System.Collections;
using Lucky;
using System;
using System.Collections.Generic;

public class AdjacencyList<T>
{
    private int num_edges = 0;
    private int node_num = 0;
    List<Vertex<T>> items; //图的顶点集合
    public AdjacencyList() : this(10) { } //构造方法
    public AdjacencyList(int capacity) //指定容量的构造方法
    {
        items = new List<Vertex<T>>(capacity);
    }

    public List<int> GetAllNodeNum()
    {
        List<int> res = new List<int>();

        foreach (Vertex<T> node in items)
        {
            res.Add(node.node_num);   
        }
        return res;
    }

    public void AddVertex(T item) //添加一个顶点
    {   //不允许插入重复值
        if (Contains(item))
        {
            throw new ArgumentException("插入了重复顶点！");
        }
        items.Add(new Vertex<T>(item, node_num++));
    }
    public void AddEdge(T from, T to) //添加无向边
    {
        Vertex<T> fromVer = Find(from); //找到起始顶点
        if (fromVer == null)
        {
            throw new ArgumentException("头顶点并不存在！");
        }
        Vertex<T> toVer = Find(to); //找到结束顶点
        if (toVer == null)
        {
            throw new ArgumentException("尾顶点并不存在！");
        }
        //无向边的两个顶点都需记录边信息
        AddDirectedEdge(fromVer, toVer);
        // AddDirectedEdge(toVer, fromVer);
    }
    public bool Contains(T item) //查找图中是否包含某项
    {
        foreach (Vertex<T> v in items)
        {
            if (v.data.Equals(item))
            {
                return true;
            }
        }
        return false;
    }

    public Vertex<T> Find(T item) //查找指定项并返回
    {
        foreach (Vertex<T> v in items)
        {
            if (v.data.Equals(item))
            {
                return v;
            }
        }
        return null;
    }
    //添加有向边
    private void AddDirectedEdge(Vertex<T> fromVer, Vertex<T> toVer)
    {
        if (fromVer.firstEdge == null) //无邻接点时
        {
            fromVer.firstEdge = new Node(toVer);
        }
        else
        {
            Node tmp, node = fromVer.firstEdge;
            do
            {   //检查是否添加了重复边
                if (node.adjvex.data.Equals(toVer.data))
                {
                    throw new ArgumentException("添加了重复的边！");
                }
                tmp = node;
                node = node.next;
            } while (node != null);
            tmp.next = new Node(toVer); //添加到链表未尾
        }
    }
    public override string ToString() //仅用于测试
    {   //打印每个节点和它的邻接点
        string s = string.Empty;
        foreach (Vertex<T> v in items)
        {
            s += v.data.ToString() + ":";
            if (v.firstEdge != null)
            {
                Node tmp = v.firstEdge;
                while (tmp != null)
                {
                    s += tmp.adjvex.data.ToString();
                    tmp = tmp.next;
                }
            }
            s += "\r\n";
        }
        return s;
    }
    //嵌套类，表示链表中的表结点
    public class Node
    {
        public Vertex<T> adjvex; //邻接点域
        public Node next; //下一个邻接点指针域
        public Node(Vertex<T> value)
        {
            adjvex = value;
        }
    }
    //嵌套类，表示存放于数组中的表头结点
    public class Vertex<TValue>
    {
        public TValue data; //数据
        public Node firstEdge; //邻接点链表头指针
        public Boolean visited; //访问标志,遍历时使用
        public int node_num;

        public Vertex(TValue value, int num) //构造方法
        {
            data = value;
            node_num = num;
        }
    }
}

public class CityMapping {
    public int edge_num;
    public string start_node;
    public string end_node;
    public CityMapping(int num, string start_node, string end_node)
    {
        this.edge_num = num;
        this.start_node = start_node;
        this.end_node = end_node;
    }
}



public class CityUtil : BaseInstance<CityUtil> {
    private Dictionary<int, string> city_dict = new Dictionary<int, string>();

    private AdjacencyList<string> city_list = new AdjacencyList<string>();
    private Dictionary<int, CityMapping> city_mapping_list = new Dictionary<int, CityMapping>();
    private Dictionary<string, List<string>> city_tikcet_mapping = new Dictionary<string, List<string>>();

    public CityUtil()
    {
        city_dict.Add(0, "沈阳");
        city_dict.Add(1, "北京");
        city_dict.Add(2, "天津");
        city_dict.Add(3, "石家庄");
        city_dict.Add(4, "济南");
        city_dict.Add(5, "郑州");
        city_dict.Add(6, "南京");
        city_dict.Add(7, "杭州");
        city_dict.Add(8, "上海");
        city_dict.Add(9, "合肥");

        city_list.AddVertex("沈阳");
        city_list.AddVertex("北京");
        city_list.AddVertex("天津");
        city_list.AddVertex("石家庄");
        city_list.AddVertex("济南");
        city_list.AddVertex("郑州");
        city_list.AddVertex("南京");
        city_list.AddVertex("杭州");
        city_list.AddVertex("上海");
        city_list.AddVertex("合肥");

        city_list.AddEdge("合肥", "杭州");
        city_mapping_list[0] = new CityMapping(0, "合肥", "杭州");

        city_list.AddEdge("合肥", "郑州");
        city_mapping_list[1] = new CityMapping(1, "合肥", "郑州");

  
        city_list.AddEdge("杭州", "郑州");
        city_mapping_list[2] = new CityMapping(3, "杭州", "郑州");

        city_list.AddEdge("杭州", "南京");
        city_mapping_list[3] = new CityMapping(4, "杭州", "南京");

        city_list.AddEdge("杭州", "上海");
        city_mapping_list[4] = new CityMapping(5, "杭州", "上海");


        city_list.AddEdge("郑州", "南京");
        city_mapping_list[5] = new CityMapping(8, "郑州", "南京");

        city_list.AddEdge("郑州", "石家庄");
        city_mapping_list[6] = new CityMapping(9, "郑州", "石家庄");


        city_list.AddEdge("南京", "石家庄");
        city_mapping_list[7] = new CityMapping(11, "南京", "石家庄");

        city_list.AddEdge("南京", "天津");
        city_mapping_list[8] = new CityMapping(12, "南京", "天津");

        city_list.AddEdge("南京", "济南");
        city_mapping_list[9] = new CityMapping(13, "南京", "济南");

        city_list.AddEdge("南京", "上海");
        city_mapping_list[10] = new CityMapping(14, "南京", "上海");


        city_list.AddEdge("上海", "济南");
        city_mapping_list[11] = new CityMapping(17, "上海", "济南");


        city_list.AddEdge("石家庄", "济南");
        city_mapping_list[12] = new CityMapping(20, "石家庄", "济南");


        city_list.AddEdge("石家庄", "北京");
        city_mapping_list[13] = new CityMapping(21, "石家庄", "北京");

        city_list.AddEdge("济南", "天津");
        city_mapping_list[14] = new CityMapping(25, "济南", "天津");


        city_list.AddEdge("北京", "沈阳");
        city_mapping_list[15] = new CityMapping(27, "北京", "沈阳");

        city_list.AddEdge("天津", "沈阳");
        city_mapping_list[16] = new CityMapping(29, "天津", "沈阳");

        city_tikcet_mapping["上海"] = new List<string>();
        city_tikcet_mapping["上海"].Add("杭州");
        city_tikcet_mapping["上海"].Add("天津");
        city_tikcet_mapping["上海"].Add("南京");

        city_tikcet_mapping["杭州"] = new List<String>();
        city_tikcet_mapping["杭州"].Add("南京");

        city_tikcet_mapping["南京"] = new List<string>();
        city_tikcet_mapping["南京"].Add("合肥");
        city_tikcet_mapping["南京"].Add("郑州");

        city_tikcet_mapping["合肥"] = new List<String>();
        city_tikcet_mapping["合肥"].Add("郑州");
        city_tikcet_mapping["合肥"].Add("济南");

        city_tikcet_mapping["郑州"] = new List<String>();
        city_tikcet_mapping["郑州"].Add("石家庄");
        city_tikcet_mapping["郑州"].Add("济南");


        city_tikcet_mapping["济南"] = new List<String>();
        city_tikcet_mapping["济南"].Add("石家庄");
        city_tikcet_mapping["济南"].Add("天津");

        city_tikcet_mapping["石家庄"] = new List<String>();
        city_tikcet_mapping["石家庄"].Add("北京");

        city_tikcet_mapping["北京"] = new List<String>();
        city_tikcet_mapping["北京"].Add("沈阳");


        city_tikcet_mapping["天津"] = new List<String>();
        city_tikcet_mapping["天津"].Add("北京");
        city_tikcet_mapping["天津"].Add("沈阳");


    }

    AdjacencyList<string>.Vertex<string> FindConnectedCity(string city)
    {
        return city_list.Find(city);
    }

    public List<int> GetAllCityNodeNum()
    {
        List<int> res = new List<int>();
        foreach(KeyValuePair<int, string> pair in city_dict)
        {
            res.Add(pair.Key);
        }
        return res;
    }

    public List<int> GetAllCityEdgeNum()
    {
        List<int> res = new List<int>();
        foreach (KeyValuePair<int, CityMapping> pair in city_mapping_list)
        {
            res.Add(pair.Key);
        }
        return res;
    }
    public CityMapping GetEdgeCity(int ID)
    {
        return city_mapping_list[ID];
    }
    public string GetCityName(int ID)
    {
        //Lucky.LuckyUtils.Log(ID);
        return city_dict[ID];
    }

    public int GetRevertCityMapping(int id)
    {
        string start_node = city_mapping_list[id].start_node;
        string end_node = city_mapping_list[id].end_node;

        foreach (KeyValuePair<int, CityMapping> pair in city_mapping_list)
        {
            CityMapping m = pair.Value;
            if (m.start_node == end_node && m.end_node == start_node)
            {
                return m.edge_num;
            }
        }
        return -1;
    }


    List<String> GetCityList(string city)
    {
        return city_tikcet_mapping[city];
    }

}