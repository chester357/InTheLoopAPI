using InTheLoopAPI.Models;
using InTheLoopAPI.Models.Database;
using InTheLoopAPI.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Service
{
    public class TagService
    {
        DatabaseContext _dataContext;

        public TagService()
        {
            _dataContext = new DatabaseContext();
        }

        public Tag CreateTag(TagModel tagModel)
        {
            if(tagModel == null || string.IsNullOrEmpty(tagModel.TagName))
            {
                return null;
            }

            var tag = new Tag { Name = tagModel.TagName };

            var result = _dataContext.Tags.Add(tag);

            _dataContext.SaveChanges();

            return result;
        }
    }
}