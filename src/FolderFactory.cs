using System;
using DynamicRest;

namespace Puddle
{
    internal class FolderFactory
    {
        private readonly RestOperation _response;

        public FolderFactory(RestOperation response)
        {
            _response = response;
        }

        public static implicit operator Folder(FolderFactory factory)
        {
            return factory.Build();
        }

        private Folder Build()
        {
            var folder = new Folder(
                _response.Result.description,
                _response.Result.title,
                DateTime.Parse(_response.Result.created),
                DateTime.Parse(_response.Result.updated));
            return folder;
        }
    }
}