//using GroupDocs.Viewer.Interfaces;
//using System;
//using System.IO;

//public class MemoryStreamPageStream : CreatePageStream
//{
//    private readonly MemoryStream _memoryStream;

//    public MemoryStreamPageStream(MemoryStream memoryStream)
//    {
//        _memoryStream = memoryStream;
//    }

//    public Stream CreatePageStream(int pageNumber)
//    {
//        return _memoryStream;
//    }

//    public void ReleasePageStream(int pageNumber, Stream pageStream)
//    {
//        // No need to release resources for MemoryStream
//    }
//}
