 import { Component, OnInit } from '@angular/core';
 import { Loader } from "@googlemaps/js-api-loader"

declare const google: any

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit {
  private loader = new Loader({
    apiKey: "AIzaSyCjw6FViQQTE_bZto3MHgmLYlltibt9svM",
    version: "weekly"
  });

  private mapOptions = {
    center: {
      lat: 0,
      lng: 0
    },
    zoom: 4
  };
  
  constructor() { }

  ngOnInit(): void {
    this.loader.load().then(() => {
      var map = new google.maps.Map(document.getElementById("map") as HTMLElement, {
        center: { lat: -34.397, lng: 150.644 },
        zoom: 8,
      });
    });
  }

}
