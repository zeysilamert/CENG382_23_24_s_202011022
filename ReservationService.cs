using System;
using System.Collections.Generic;

public class ReservationService
{
    private readonly ReservationHandler _reservationHandler;

    public ReservationService(ReservationHandler reservationHandler)
    {
        _reservationHandler = reservationHandler;
    }

    public void AddReservation(string day, string roomNumber, string reserverName, DateTime enterTime)
    {
       _reservationHandler.AddReservation(day, roomNumber, reserverName,enterTime);
    }

    public void DeleteReservationByName(string reserverName)
    {
        _reservationHandler.DeleteReservationByName(reserverName);
    }

    public void PrintWeeklySchedule()
    {
        _reservationHandler.PrintWeeklySchedule();
    }
}
