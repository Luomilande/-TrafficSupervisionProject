//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Notice_info
    {
        public int id { get; set; }
        public string Title { get; set; }
        public string NoticeContent { get; set; }
        public string CreationDate { get; set; }
        public bool State { get; set; }
        public int Author { get; set; }
        public int Reading { get; set; }
    }
}
