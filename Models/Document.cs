//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Document
    {
        public int UID { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentName { get; set; }
        public string DocumentType { get; set; }
        public string PictureSize { get; set; }
        public string PictureResolution { get; set; }
        public string PictureBackground { get; set; }
        public string DocumentPath { get; set; }
        public string Creater { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string Editor { get; set; }
        public System.DateTime EditTime { get; set; }
        public string Remark { get; set; }
        public int State { get; set; }
    }
}
