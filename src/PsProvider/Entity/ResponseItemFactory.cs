using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using DynamicRest.Xml;
using PsHuddle.Entity.Builder;
using PsHuddle.Entity.Entities;

namespace PsHuddle.Entity
{
    public class ResponseItemFactory
    {
        public HuddleResourceObject Create(dynamic response) 
        {

            var entityType = GetEntityType(response);

            var result = response.Result as XmlNode;

            //leave it be for now wont be a string for long
            var map = new Dictionary<dynamic, Func<HuddleResourceObject>>
                          {
                              {"document", ()=> DocumentBuilder.Build(response.Result)},
                              {"folder", ()=>   FolderBuilder.Build(response.Result)},
                              {"workspace", ()=> WorkSpaceBuilder.Build(response.Result)},
                              {"user", ()=> UserBuilder.Build(response.Result)}
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
