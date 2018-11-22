// **********************************************************************
// This file was generated by a TAF parser!
// TAF version 3.2.1.2 by WSRD Tencent.
// Generated from `/data/jcetool/taf//upload/luckywei/MyNetwork.jce'
// **********************************************************************

using System;
namespace MyNetwork
{

    public sealed class NetMsg : Wup.Jce.JceStruct
    {
        double _offset = 0;
        public double offset
        {
            get
            {
                return _offset;
            }
            set
            {
                _offset = value;
            }
        }

        public System.Collections.Generic.List<byte> buffer { get; set; }

        public override void WriteTo(Wup.Jce.JceOutputStream _os)
        {
            _os.Write(offset, 0);
            _os.Write(buffer, 1);
        }

        public override void ReadFrom(Wup.Jce.JceInputStream _is)
        {
            offset = (double)_is.Read(offset, 0, true);

            buffer = (System.Collections.Generic.List<byte>)_is.Read(buffer, 1, true);

        }

        public override void Display(System.Text.StringBuilder _os, int _level)
        {
            Wup.Jce.JceDisplayer _ds = new Wup.Jce.JceDisplayer(_os, _level);
            _ds.Display(offset, "offset");
            _ds.Display(buffer, "buffer");
        }

    }
}

