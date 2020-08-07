using System.Collections.Generic;

public class Observable<T>{
    public delegate void ChangeValue(T v);
    public event ChangeValue propertyUpdated;
 
    T v;
    public T val{
        get{
            return v;
        }
        set{
            T previousValue = v;
         
            if(!EqualityComparer<T>.Default.Equals(previousValue, value)){
                v = value;

                if(propertyUpdated != null){
                    propertyUpdated(v);
                }
            }
        }
    }

    public Observable(T value){
        v = value;
    }
}

