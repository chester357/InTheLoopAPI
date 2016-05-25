using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Models.Request
{
    public class Categories
    {
        public List<TagModel> List { get; set; }

        public Categories()
        {
            List = new List<TagModel>
            {
                new TagModel{ TagName = "Music & Art", InternalId = 1 },
                new TagModel{ TagName = "Night Life", InternalId = 2 },
                new TagModel{ TagName = "Food & Drink", InternalId = 3 },
                new TagModel{ TagName = "Sports & Fitness", InternalId = 4 },
                new TagModel{ TagName = "Shopping", InternalId = 5 },
                new TagModel{ TagName = "Educational", InternalId = 6 }
            };
        }
    }
}