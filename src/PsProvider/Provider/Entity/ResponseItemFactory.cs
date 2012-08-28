using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Provider.Entity.Builder;
using Provider.Entity.Entities;
using Provider.Resource;

namespace Provider.Entity
{
    public class ResponseItemFactory
    {
        public HuddleResourceObject Create(dynamic response) 
        {

            var entityType = GetEntityType(response);

            //leave it be for now wont be a string for long
            var map = new Dictionary<dynamic, Func<HuddleResourceObject>>
                          {
                              {"document", ()=> DocumentBuilder.Build(response.Result)},
                              {"folder", ()=>   FolderBuilder.Build(response.Result)},
                              {"workspace", ()=> WorkSpaceBuilder.Build(response.Result)}
                          };

            if(map.ContainsKey(entityType))
            {
                return map[entityType]();
            }

            throw new InvalidOperationException("Entity type not recognised " + entityType);
        }

        private dynamic GetEntityType(dynamic response)
        {
            var rootElementName = response.Result.Name;
            return rootElementName;
        }
    }

}
