import { Component, OnInit, Input } from '@angular/core';
import { RoverPosition} from '../rover';
declare const Microsoft: any


@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit {
  @Input() linePosition!: RoverPosition[];

  public factor: number = 3;
  public svgHeight: number = 0;
  public svgWidth: number = 0;
  public lineBorderSize: number = 0;

  constructor() {
    this.svgHeight = 180 * this.factor;
    this.svgWidth = 360 * this.factor;
    this.lineBorderSize = 2;
  }

  ngOnInit() {

  }
}

