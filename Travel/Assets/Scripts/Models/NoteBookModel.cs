using UnityEngine;
using System.Collections;
using Lucky;
using System.Collections.Generic;
using System;

public class OnePageNoteBook
{
    public DateTime time;
    public string imagePath;
    public string FirstText;
    public List<string> buttontext = new List<string>();
    public List<string> finaltext = new List<string>();
    public int chosen = -1;

    public OnePageNoteBook(Event data,DateTime ttime,string image)
    {
        time = ttime;
        imagePath = image;
        FirstText = data.content[0].text;
        for(int i = 1; i < data.content.Count; i++)
        {
            buttontext.Add(data.content[i].condition);
            finaltext.Add(data.content[i].text);
        }
    }
}

public class NoteBookModel : BaseInstance<NoteBookModel> {

    public Dictionary<string, List<OnePageNoteBook>> data = new Dictionary<string, List<OnePageNoteBook>>();

    public List<OnePageNoteBook> noteBookList = new List<OnePageNoteBook>();

    public NoteBookModel()
    {

    }
}
