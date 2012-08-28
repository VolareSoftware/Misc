using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public static class ICollectionExtensions
    {
        public static void AddByIds<T>(this ICollection<T> collection, IEnumerable<int> newlySelectedIds, IUnitOfWork unitOfWork) where T : TrackedEntity, new()
        {
            // Remove the items that are no longer selected
            var previouslySelectedIds = collection.Select(i => i.Id);
            foreach (var id in previouslySelectedIds.ToList().Except(newlySelectedIds))
            {
                var itemToRemove = collection.Where(i => i.Id == id).Single();
                collection.Remove(itemToRemove);
            }

            // Add newly selected items
            foreach (var id in newlySelectedIds.Except(previouslySelectedIds))
            {
                var entity = new T {Id = id};
                collection.Add(entity);
                unitOfWork.Attach(entity);
            }
        }

        public static void ForEach<T>(this ICollection<T> collection, Action<T> action)
        {
            foreach (T item in collection)
            {
                action(item);
            }
        }
    }
}