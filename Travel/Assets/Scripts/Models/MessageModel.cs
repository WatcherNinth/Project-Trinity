using UnityEngine;
using System.Collections;
using Lucky;
using System.Collections.Generic;

public class MessageModel : BaseInstance<MessageModel> {

    public List<WeChatMessage> WeChatList = new List<WeChatMessage>();
    public List<NewMessage> NewsList = new List<NewMessage>();
}
