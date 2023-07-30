import { Component, OnInit } from '@angular/core';
import { FlightService } from './../api/services/flight.service';
import { FlightRm } from '../api/models';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-search-flights',
  templateUrl: './search-flights.component.html',
  styleUrls: ['./search-flights.component.css']
})
export class SearchFlightsComponent implements OnInit {

  searchResult: FlightRm[] = [];

  constructor(
    private flightService: FlightService,
    private fb: FormBuilder,
  ) { }

  searchForm = this.fb.group({
    fromDate: [''],
    toDate: [''],
    departure: [''],
    destination: [''],
    numOfPassengers: [1]
  });

  ngOnInit(): void {
    this.search();
  }

  search() {
    this.flightService.searchFlight(this.searchForm.value as any) //should not be "as any"
      .subscribe(response => this.searchResult = response, this.handleError)
  }

  private handleError(err: any) {
    console.log("Response Error. Status: ", err.status)
    console.log("Response Error. Status Text: ", err.statusText)
    console.log(err);
  }
}
