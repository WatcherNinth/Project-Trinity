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

    public OnePageNoteBook(DateTime ttime,string image, string firstText)
    {
        time = ttime;
        imagePath = image;
        FirstText = firstText;
    }
}

public class NoteBookModel : BaseInstance<NoteBookModel> {

    public Dictionary<string, List<OnePageNoteBook>> data = new Dictionary<string, List<OnePageNoteBook>>();

    public List<OnePageNoteBook> noteBookList = new List<OnePageNoteBook>();

    public NoteBookModel()
    {

    }
}
