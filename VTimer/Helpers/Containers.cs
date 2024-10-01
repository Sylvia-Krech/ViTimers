using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using VTimer.Consts;

namespace VTimer.Helpers;

public enum TimestampStatus {
  up, upSoon, upEventually, past
}

public class KeyVal<K, V>{
    public K Key { get; set; }
    public V Value { get; set; }
    public KeyVal(K key, V val)
    {
        this.Key = key;
        this.Value = val;
    }
}

public class Timestamp : IComparable<Timestamp> {
    public long start { get; set; } 
    public long end { get; set; } 
    public Tracker tracker;

    public Timestamp(long a, long b, Tracker t) {
        start = a;
        end = b;
        tracker = t;
    }

    public Timestamp((long, long) time, Tracker t) : this (time.Item1, time.Item2, t) {}

    public TimestampStatus getStatus() {
        long now = EorzeanTime.now();
        if ( this.start <= now ) {
            return TimestampStatus.up;
        } else if ( this.start <= now + tracker.getForewarning() ) {
            return TimestampStatus.upSoon;
        } else if ( this.end < now ) {
            return TimestampStatus.past;
        }
        return TimestampStatus.upEventually;
    }

    public System.Numerics.Vector4 statusColor() {
        var status = this.getStatus();
        switch (status){
            case TimestampStatus.up:
                return Colors.up;
            case TimestampStatus.upSoon:
                return Colors.upSoon;
            case TimestampStatus.upEventually:
                return Colors.upEventually;
            default:
                return Colors.error;
        }
    }

    public int CompareTo(Timestamp? other)
    {
        if( this.start == other.start){
            if (this.tracker.name[0] == other.tracker.name[0]) return 0;
            if (this.tracker.name[0] > other.tracker.name[0]) return 1;
            return -1;
        }
        if( this.start > other.start) return 1;
        return -1; 
    }
}

public class Val<V> {
    public V Value { get; set; }
    public Val(V val){
        this.Value = val;
    }
}