import { Component, OnInit } from '@angular/core';
import { BookingRm, BookDto } from '../api/models';
import { BookingService } from '../api/services';
import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-my-flights',
  templateUrl: './my-flights.component.html',
  styleUrls: ['./my-flights.component.css']
})
export class MyFlightsComponent implements OnInit {

  bookings!: BookingRm[];

  constructor(
    private bookingService: BookingService,
    private authService: AuthService,
  ) { }

  ngOnInit(): void {
    this.bookingService.listBooking({ email: this.authService.currentUser?.email ?? '' })
      .subscribe(r => this.bookings = r, this.handleError);
  }

  cancel(booking: BookingRm) {

    const dto: BookDto = {
      flightId: booking.flightId,
      numOfSeats: booking.numOfBookedSeats,
      passengerEmail: booking.passengerEmail
    };

    this.bookingService.cancelBooking({ body: dto })
      .subscribe(r =>
        this.bookings = this.bookings.filter(b => b != booking),
        this.handleError
      );

  }

  private handleError(err: any) {
    console.log("Response Error, Status:", err.status);
    console.log("Response Error, Status Text:", err.statusText);
    console.log(err);
  }

}
