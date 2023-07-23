import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { FlightService } from './../api/services/flight.service';
import { AuthService } from '../auth/auth.service';
import { BookDto, FlightRm } from '../api/models';

@Component({
  selector: 'app-book-flight',
  templateUrl: './book-flight.component.html',
  styleUrls: ['./book-flight.component.css']
})
export class BookFlightComponent implements OnInit {

  flightId: string = 'not loaded';
  flight: FlightRm = {}

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private flightService: FlightService,
    private auth: AuthService,
    private fb: FormBuilder,
  ) { }

  form = this.fb.group({
    number: [
      1,
      Validators.compose([
        Validators.required,
        Validators.min(1),
        Validators.max(254)]
      )]
  });

  ngOnInit(): void {
    this.route.paramMap
      .subscribe(p => this.findFlight(p.get("flightId")));
  }

  private findFlight = (flightId: string | null) => {
    this.flightId = flightId ?? 'not passed';

    this.flightService.findFlight({ id: this.flightId })
      .subscribe(flight => this.flight = flight, this.handleError);
  }

  private handleError = (err: any) => {

    if (err.status == 404) {
      alert("Flight Not Found!")
      this.router.navigate(['/search-flights'])
    }

    if (err.status == 409) {
      console.log("err: " + err);
      alert(JSON.parse(err.error).message);
    }

    console.log("Response Error. Status: ", err.status)
    console.log("Response Error. Status Text: ", err.statusText)
    console.log(err);
  }

  book() {

    if (this.form.invalid)
      return;

    console.log(`Booking ${this.form.get('number')?.value} passengers for the flight: ${this.flight.id}`);

    const booking: BookDto = {
      flightId: this.flight.id,
      passengerEmail: this.auth.currentUser?.email as string,
      numOfSeats: this.form.get('number')?.value as number
    }

    console.log(booking);

    this.flightService.bookFlight({ body: booking})
      .subscribe(_ => this.router.navigate(['/my-flights']),
        this.handleError)
  }

  get number() {
    return this.form.controls.number;
  }

}
