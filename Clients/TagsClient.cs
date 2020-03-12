using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Modio.Models;

namespace Modio
{
    using Parameter = KeyValuePair<string, string>;

    public class TagsClient : ApiClient
    {
        public uint GameId { get; private set; }

        public uint ModId { get; private set; }

        internal TagsClient(IConnection connection, uint game, uint mod) : base(connection)
        {
            GameId = game;
            ModId = mod;
        }

        public async Task<IReadOnlyList<Tag>> Get()
        {
            var route = Routes.GetModTags(GameId, ModId);
            var search = new SearchClient<Tag>(Connection, route, null);
            return await search.ToList();
        }

        public async Task Add(params string[] tags)
        {
            await Add((IEnumerable<string>)tags);
        }

        public async Task Add(IEnumerable<string> tags)
        {
            var (method, path) = Routes.AddModTags(GameId, ModId);
            var req = new Request(method, Connection.BaseAddress, path);

            var parameters = tags.Select(t => new Parameter("tags[]", t));
            req.Body = new FormUrlEncodedContent(parameters);

            await Connection.Send<ApiMessage>(req);
        }

        public async Task Delete(params string[] tags)
        {
            await Delete((IEnumerable<string>)tags);
        }

        public async Task Delete(IEnumerable<string> tags)
        {
            var (method, path) = Routes.DeleteModTags(GameId, ModId);
            var req = new Request(method, Connection.BaseAddress, path);

            var parameters = tags.Select(t => new Parameter("tags[]", t));
            req.Body = new FormUrlEncodedContent(parameters);

            await Connection.Send<ApiMessage>(req);
        }
    }
}
