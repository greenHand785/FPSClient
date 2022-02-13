using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LimitedStack<T>
{
    private List<T> stack;
    private int size;
    public int Size
    {
        get
        {
            return size;
        }
        set
        {
            size = value;
        }
    }
    public List<T> Stack
    {
        get
        {
            return stack;
        }
        set
        {
            if (value == null)
            {
                return;
            }
            stack = value;
        }
    }
    public LimitedStack(int size)
    {
        Size = size;
    }

    public void Push(T value)
    {
        if(stack == null)
        {
            return;
        }
        if(stack.Count >= size)
        {
            stack.RemoveAt(0);
            stack.Add(value);
            return;
        }
        stack.Add(value);
    }

    public T Pop()
    {
        if(stack == null)
        {
            return default(T);
        }
        T t = stack[stack.Count - 1];
        stack.RemoveAt(stack.Count - 1);
        return t;
    }
}

