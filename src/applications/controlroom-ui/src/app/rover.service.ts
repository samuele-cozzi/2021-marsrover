import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';

import { RoverPosition } from './rover';

@Injectable({
  providedIn: 'root'
})
export class RoverService {

  private urlPrefix = 'https://localhost:44373/api/rover';  // URL to web api
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  getRoverPositions(): Observable<RoverPosition[]> {
    const url = `${this.urlPrefix}/position`;
    return this.http.get<RoverPosition[]>(url);
  }

  getRoverPosition(): Observable<RoverPosition> {
    const url = `${this.urlPrefix}/position/last`;
    return this.http.get<RoverPosition>(url);
  }

  start(command: string[]): any {
    const url = `${this.urlPrefix}/move`
    return this.http.post(url, command, this.httpOptions);
  }

  
}
