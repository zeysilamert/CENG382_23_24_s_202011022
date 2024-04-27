using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;

public class ReservationRepository : IReservationRepository
{
    private readonly List<Reservation> reservations = new List<Reservation>();
    private readonly string reservationDataFilePath;
    private readonly ILogger logger;

    public ReservationRepository(string dataFile, ILogger logger)
    {
        this.reservationDataFilePath = dataFile;
        this.logger = logger;   
    }

    public void AddReservation(Reservation reservation)
    {
        reservations.Add(reservation);
        UpdateReservationDataFile();
    }

    public void DeleteReservation(string reserverName)
    {
        reservations.RemoveAll(r => r.ReserverName == reserverName);
        UpdateReservationDataFile();
    }

    public IEnumerable<Reservation> GetAllReservations()
    {
        return reservations;
    }

    private void UpdateReservationDataFile(){
        var reservationJson = JsonSerializer.Serialize(reservations, new JsonSerializerOptions {WriteIndented =true});
        File.WriteAllText(reservationDataFilePath, reservationJson);
    } 
}