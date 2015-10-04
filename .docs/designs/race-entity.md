
## Race

 - We don't record the round as it can be identified by the date order.
 - We don't record the offset of the drivers to the first driver as it can be identified by the positio. So, we only store the race finish time. 

```
Race
    Id
    Name
    Season:{ref}
    Circuit:{ref}
    RaceDate
    RaceTimeInUtc
    RaceTimeInLocal
    Url

    Result:{}
        RaceTime
        NumberOfRacedLaps
        Standings:[]
            Car:{}
                Number
                Driver:{ref}
                Constructor:{ref}
            GridPosition
            RacePosition (the standing number to represent the position given, could be 16, 20, etc.)
            RaceResult:{} (the race result, could be 1, DNF, DSQ, etc.)
                Status
                RaceFinishTime
                PointsGained
```
