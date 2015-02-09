﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zinkuba.App.Mailbox;

namespace Zinkuba.App.MailAccount
{
    public class MboxAccount : IMailAccount
    {
        private readonly ObservableCollection<MboxMailbox> _mailboxes;

        public MboxAccount()
        {
            _mailboxes = new ObservableCollection<MboxMailbox>() { new MboxMailbox(this) };
            Mailboxes = new ReturnTypeCollection<IMailbox>();
            Mailboxes.UnderlyingCollection = _mailboxes;
            _mailboxes.CollectionChanged += MailboxesOnCollectionChanged;
        }

        private void MailboxesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if (notifyCollectionChangedEventArgs.NewItems != null)
            {
                foreach (var newItem in notifyCollectionChangedEventArgs.NewItems)
                {
                    OnAddedMailbox((MboxMailbox)newItem);
                }
            }
            if (notifyCollectionChangedEventArgs.OldItems != null)
            {
                foreach (var oldItem in notifyCollectionChangedEventArgs.OldItems)
                {
                    OnRemovedMailbox((MboxMailbox)oldItem);
                }
            }
        }

        public event EventHandler<IMailbox> AddedMailbox;
        public event EventHandler<IMailbox> RemovedMailbox;
        public ReturnTypeCollection<IMailbox> Mailboxes { get; private set; }

        protected virtual void OnRemovedMailbox(IMailbox e)
        {
            EventHandler<IMailbox> handler = RemovedMailbox;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnAddedMailbox(IMailbox e)
        {
            EventHandler<IMailbox> handler = AddedMailbox;
            if (handler != null) handler(this, e);
        }
    }
}
