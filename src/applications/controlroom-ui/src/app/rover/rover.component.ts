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

  turnLeft(): void {
    this.commands.push('l');
    this.commands.push('f');
  }

  turnRight(): void {
    this.commands.push('r');
    this.commands.push('f');
  }

  send(): void {
    this.reoverService.start(this.commands)
      .subscribe(() => {});
    this.commands = [];
  }

  explore(): void {
    this.reoverService.explore(this.commands)
      .subscribe(() => {});
    this.commands = [];
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
