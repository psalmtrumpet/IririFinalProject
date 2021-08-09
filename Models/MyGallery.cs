using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IririFinalProject.Models
{
    public class MyGallery
    {
        public Guid FileId { get; set; }
        public string FileName { get; set; }

        public string FileTitle { get; set; }


        public string FilePicture { get; set; }


        public string Description { get; set; }

        public string Event { get; set; }
    }
}
