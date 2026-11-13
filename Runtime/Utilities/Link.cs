using System.Collections.Generic;

namespace VAT.Shared.Utilities
{
    /// <summary>
    /// A link that can connect to other links and be traversed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Link<T>
    {
        /// <summary>
        /// The object this link is assigned to.
        /// </summary>
        public T Origin { get; }

        /// <summary>
        /// The other links that this link is connected to directly.
        /// </summary>
        public List<Link<T>> ConnectedLinks { get; } = new();

        /// <summary>
        /// The ID of this link that can be used to track graph traversal.
        /// </summary>
        public int ID { get; } = _lastID++;

        private static int _lastID = 0;

        /// <summary>
        /// Creates a new link for an object.
        /// </summary>
        /// <param name="origin"></param>
        public Link(T origin)
        {
            Origin = origin;
        }

        /// <summary>
        /// Connects this link both ways to another link.
        /// </summary>
        /// <param name="other"></param>
        public void Connect(Link<T> other)
        {
            ConnectOneWay(other);
            other.ConnectOneWay(this);
        }

        /// <summary>
        /// Disconnects this link both ways from another link.
        /// </summary>
        /// <param name="other"></param>
        public void Disconnect(Link<T> other)
        {
            DisconnectOneWay(other);
            other.DisconnectOneWay(this);
        }

        /// <summary>
        /// Gets all of the links that branch out from this link.
        /// </summary>
        /// <returns></returns>
        public List<Link<T>> GetAllLinks() => GetAllLinks(int.MaxValue);

        /// <summary>
        /// Gets all of the links that branch out from this link up to a maximum search depth.
        /// </summary>
        /// <param name="maxDepth"></param>
        /// <returns></returns>
        public List<Link<T>> GetAllLinks(int maxDepth)
        {
            List<Link<T>> allLinks = new();
            HashSet<int> visitedLinks = new();

            GetAllLinksRecursive(allLinks, visitedLinks, 0, maxDepth);

            return allLinks;
        }

        /// <summary>
        /// Gets all of the links at a specific search depth from this link.
        /// </summary>
        /// <param name="depth"></param>
        /// <returns></returns>
        public List<Link<T>> GetLinksAtDepth(int depth)
        {
            List<Link<T>> linksAtDepth = new();
            HashSet<int> addedLinks = new();

            GetLinksAtDepthRecursive(linksAtDepth, addedLinks, 0, depth);

            return linksAtDepth;
        }

        private void GetAllLinksRecursive(List<Link<T>> allLinks, HashSet<int> visitedLinks, int depth, int maxDepth)
        {
            if (!visitedLinks.Add(ID))
            {
                return;
            }

            allLinks.Add(this);

            if (depth >= maxDepth)
            {
                return;
            }

            foreach (var link in ConnectedLinks)
            {
                link.GetAllLinksRecursive(allLinks, visitedLinks, depth + 1, maxDepth);
            }
        }

        private void GetLinksAtDepthRecursive(List<Link<T>> linksAtDepth, HashSet<int> addedLinks, int currentDepth, int targetDepth)
        {
            if (currentDepth == targetDepth && addedLinks.Add(ID))
            {
                linksAtDepth.Add(this);
            }

            if (currentDepth < targetDepth)
            {
                foreach (var link in ConnectedLinks)
                {
                    link.GetLinksAtDepthRecursive(linksAtDepth, addedLinks, currentDepth + 1, targetDepth);
                }
            }
        }

        private void ConnectOneWay(Link<T> other)
        {
            ConnectedLinks.Add(other);
        }

        private void DisconnectOneWay(Link<T> other)
        {
            ConnectedLinks.Remove(other);
        }
    }
}
