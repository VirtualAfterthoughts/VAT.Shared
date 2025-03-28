using System.Collections.Generic;

namespace VAT.Shared.Utilities
{
    public class Link<T>
    {
        public enum LinkType
        {
            /// <summary>
            /// A directly connected link.
            /// </summary>
            PRIMARY,

            /// <summary>
            /// A link connected via a primary link.
            /// </summary>
            SECONDARY
        }

        public delegate void LinkCallback(Link<T> from, Link<T> to, LinkType type);

        private readonly List<Link<T>> _primaryLinks = new();

        /// <summary>
        /// The links that are directly connected to this Link.
        /// </summary>
        public List<Link<T>> PrimaryLinks => _primaryLinks;

        private readonly List<Link<T>> _secondaryLinks = new();

        /// <summary>
        /// The links that are connected to this Link through its primary links.
        /// </summary>
        public List<Link<T>> SecondaryLinks => _secondaryLinks;

        public event LinkCallback OnLinkConnected, OnLinkDisconnected;

        /// <summary>
        /// The object this Link is assigned to.
        /// </summary>
        public T Origin { get; }

        public Link(T origin)
        {
            Origin = origin;
        }

        private void HookLink(Link<T> link)
        {
            link.OnLinkConnected += LinkConnected;
            link.OnLinkDisconnected += LinkDisconnected;
        }

        private void UnhookLink(Link<T> link)
        {
            link.OnLinkConnected -= LinkConnected;
            link.OnLinkDisconnected -= LinkDisconnected;
        }

        public void Connect(Link<T> other)
        {
            HookLink(other);
            other.HookLink(this);

            ConnectOneWay(other);
            other.ConnectOneWay(this);
        }

        private void ConnectOneWay(Link<T> other)
        {
            _primaryLinks.Add(other);

            OnLinkConnected?.Invoke(this, other, LinkType.PRIMARY);
        }

        public void Disconnect(Link<T> other)
        {
            DisconnectOneWay(other);
            other.DisconnectOneWay(this);

            UnhookLink(other);
            other.UnhookLink(this);
        }

        private void DisconnectOneWay(Link<T> other)
        {
            _primaryLinks.Remove(other);

            OnLinkDisconnected?.Invoke(this, other, LinkType.PRIMARY);
        }

        private void LinkConnected(Link<T> from, Link<T> to, LinkType type)
        {
            if (type != LinkType.PRIMARY)
            {
                return;
            }

            if (to == this)
            {
                return;
            }

            bool alreadyLinked = _secondaryLinks.Contains(to);

            _secondaryLinks.Add(to);

            if (!alreadyLinked)
            {
                OnLinkConnected?.Invoke(this, to, LinkType.SECONDARY);
            }
        }

        private void LinkDisconnected(Link<T> from, Link<T> to, LinkType type)
        {
            if (type != LinkType.PRIMARY)
            {
                return;
            }

            if (to == this)
            {
                return;
            }

            _secondaryLinks.Remove(to);

            if (!_secondaryLinks.Contains(to))
            {
                OnLinkDisconnected?.Invoke(this, to, LinkType.SECONDARY);
            }
        }
    }
}
