using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Ionic.Zip;
using ZipFile =Ionic.Zip.ZipFile;

namespace EndlessLegendEditor
{
    public class ZipSave
    {
        private readonly ZipFile archive;
        private readonly ZipEntry entry;
        private const string name = "Endless Legend/Game.xml";
        public XElement Document { get; private set; }

        public ZipSave(string path)
        {
            archive = ZipFile.Read(path);
            entry = archive[name];
            //archive = ZipFile.Open(path,ZipArchiveMode.Update,Encoding.Default);
            //entry = archive.GetEntry(name);
            using (var sr = new StreamReader(entry.OpenReader()))
            {
                var raw = sr.ReadToEnd();
                Document = XElement.Parse(raw);
            }
        }

        public void Save()
        {
            
            //archive.UpdateEntry(name, Document.ToString());
            //using (var stream = entry.Open())
            //{
            //    stream.Seek(0, SeekOrigin.End);
            //    using (var sw = new StreamWriter(stream))
            //    {
            //        sw.Write(Document.ToString());
            //        sw.Flush();
            //    }
            //}
            var path = AppDomain.CurrentDomain.BaseDirectory + "temp.xml";
            Document.Save(path);
            archive.RemoveEntry(entry);
            archive.AddEntry(name, new FileStream(path, FileMode.Open));
            archive.Save();
            //using (var s = new MemoryStream())
            //{
            //    using (var sw = new StreamWriter(s))
            //    {
            //        var xml = Document.ToString();
            //        sw.WriteLine("123");
            //        sw.Flush();
            //        archive.AddEntry("test.xml", s);
            //        archive.Save();
            //    }
            //    //Document.Save(s);
                
            //}
        }
    }
}
