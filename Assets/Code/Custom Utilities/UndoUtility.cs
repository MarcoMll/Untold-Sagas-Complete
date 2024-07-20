using System;
using System.Collections.Generic;

namespace CustomUtilities
{
    public static class UndoUtility
    {
        private struct UndoRecord
        {
            public string Id;
            public object Value;
            public Action<object> ApplyCallback;
        }

        private static readonly Stack<UndoRecord> UndoStack = new Stack<UndoRecord>();

        public static void Record(object value, string id, Action<object> applyCallback)
        {
            UndoStack.Push(new UndoRecord { Id = id, Value = value, ApplyCallback = applyCallback });
        }

        // Perform an undo using a specific ID
        public static void Undo(string id)
        {
            foreach (var record in UndoStack)
            {
                if (record.Id == id)
                {
                    record.ApplyCallback(record.Value);
                    UndoStack.Pop();
                    break;
                }
            }
        }

        // Overloaded method to perform the last undo without specifying an ID
        public static void Undo()
        {
            if (UndoStack.Count > 0)
            {
                var lastEntry = UndoStack.Pop();
                lastEntry.ApplyCallback(lastEntry.Value);
            }
        }
    }
}