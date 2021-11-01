import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RoverPosition} from '../rover';
import { RoverService } from '../rover.service';

@Component({
  selector: 'app-rover',
  templateUrl: './rover.component.html',
  styleUrls: ['./rover.component.css']
})
export class RoverComponent implements OnInit {

  commands: string[] = [];
  position: RoverPosition | undefined;
  positions: RoverPosition[] = [];

  constructor(private reoverService: RoverService) { }

  ngOnInit(): void {
    this.refresh();
  }

  add(command: string): void {
    this.commands.push(command);
  }

  send(): void {
    this.reoverService.start(this.commands)
      .subscribe(() => {});
  }

  clear(): void {
    this.commands = [];
  }

  refresh(): void {
    this.reoverService.getRoverPosition()
      .subscribe(position => this.position = position);

    this.reoverService.getRoverPositions()
      .subscribe(positions => this.positions = positions);
  }

}