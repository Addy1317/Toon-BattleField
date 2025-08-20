using System.Collections.Generic;

/// <summary>
/// A simple generic Priority Queue where items are dequeued based on the lowest priority value (min-heap behavior).
/// </summary>
public class PriorityQueue<T>
{
    private List<(T item, int priority)> elements = new();

    public int Count => elements.Count;

    // Adds an item to the queue with a given priority.
    public void Enqueue(T item, int priority)
    {
        elements.Add((item, priority));
    }

    // Removes and returns the item with the lowest priority value.
    public T Dequeue()
    {
        int bestIndex = 0;
        for (int i = 1; i < elements.Count; i++)
        {
            if (elements[i].priority < elements[bestIndex].priority)
                bestIndex = i;
        }

        T bestItem = elements[bestIndex].item;
        elements.RemoveAt(bestIndex);
        return bestItem;
    }

    // Checks if the queue contains a specific item.
    public bool Contains(T item)
    {
        return elements.Exists(e => EqualityComparer<T>.Default.Equals(e.item, item));
    }
}
