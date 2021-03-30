import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { PolygonResult } from '../models/polygon';
import { Polygon } from '../gmap-container/gmap-container.component';

@Injectable({ providedIn: 'root' })
export class PolygonService {

  baseURL: string = "https://localhost:44315/polygonservice/";

  constructor(private client: HttpClient) { }

  getAll(): Observable<PolygonResult[]> {
    return this.client.get<PolygonResult[]>(this.baseURL)
  }

  add(polygon: Polygon) {
    return this.client.post(`${this.baseURL}`, polygon, { 'headers': { 'content-type': 'application/json' } });
  }

  update(polygon: Polygon) {
    return this.client.post(`${this.baseURL}${polygon.Id}`, polygon, { 'headers': { 'content-type': 'application/json' } });
  }

  delete(polygon: Polygon) {
    return this.client.delete(`${this.baseURL}${polygon.Id}`, { 'headers': { 'content-type': 'text/plain' } });
  }

  deleteAll() {
    return this.client.delete(`${this.baseURL}`, { 'headers': { 'content-type': 'text/plain' } });
  }
}
