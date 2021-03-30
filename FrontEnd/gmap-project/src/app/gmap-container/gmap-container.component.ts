import { Component, OnInit, HostListener } from '@angular/core';
import { MenuItem } from 'primeng/api/menuitem';
import { Message } from 'primeng/api';
declare var google: any;

import * as Wkt from 'wicket';

import { PolygonService } from 'src/app/services/polygonservice';

@Component({
  selector: 'app-gmap-container',
  templateUrl: './gmap-container.component.html',
  styleUrls: ['./gmap-container.component.css']
})
export class GmapContainerComponent implements OnInit {

  map: any;
  polys: Polygon[];
  polyModel: Polygon;
  polyOptions: MenuItem[];
  options: any;
  infowindow: any;
  drawingManager: any;
  selectedPolygon: any;
  myPolygons: any;// google.maps.Polygon[];
  overlays: any;
  msgs: Message[];

  constructor(private api: PolygonService) { }

  ngOnInit() {

    this.polyOptions = [
      {
        label: 'Reload & Center Map', command: () => {
          this.reloadAndCenter();
        }
      },
      {
        label: 'Delete All Polygons', command: () => {
          this.handleDeleteAllPolygons();
        }
      },
      { separator: true }
    ];

    this.options = {
      center: { lat: -30.5997, lng: 23.5133 },
      zoom: 7
    };

    this.infowindow = new google.maps.InfoWindow();
    this.myPolygons = [];
    this.polys = [];
  }

  @HostListener('document:click', ['$event'])
  clickout(event) {
    switch (event.target.id) {
      case 'polySave':
        this.handleSavePolygon();
        break;
      case 'polyDelete':
        this.handleDeletePolygon();
        break;
      default: break;
    }
  }

  setMap(event) {
    this.map = event.map;
    this.drawingManager = new google.maps.drawing.DrawingManager({
      drawingControlOptions: {
        position: google.maps.ControlPosition.TOP_CENTER,
        drawingModes: [google.maps.drawing.OverlayType.POLYGON],
      },
      polygonOptions: {
        fillColor: "#DC143C",
        fillOpacity: 0.5,
        strokeWeight: 2,
        editable: true,
        zIndex: 1
      },
    });

    this.drawingManager.setMap(this.map);

    var component = this;
    google.maps.event.addListener(this.drawingManager, 'polygoncomplete', function (poly) {

      component.drawingManager.setDrawingMode(null);

      let owner = new Polygon(null, '', null, null);
      poly.Owner = owner;
      if (component.selectedPolygon) {
        component.selectedPolygon.setOptions({ editable: false });
      }
      component.selectedPolygon = poly;

      google.maps.event.addListener(poly, 'click', function () {
        component.handlePolyClick(poly);
      });
      google.maps.event.trigger(component.selectedPolygon, 'click', null);

      component.myPolygons.push(poly);
    });

    this.loadPolygons();
  }

  loadPolygons() {
    var component = this;
    var wkt = new Wkt.Wkt();
    this.api.getAll().subscribe(data => {
      data.forEach(p => {
        wkt.read(p.Wkt);
        var path = this.getPolyPath(wkt.components[0]);
        var gmPoly = new google.maps.Polygon({
          paths: path,
          fillColor: "#DC143C",
          fillOpacity: 0.5,
          strokeWeight: 2,
          editable: false,
          zIndex: 1
        });

        gmPoly.setMap(this.map);

        gmPoly.Owner = new Polygon(p.Id, p.Name, p.Data, null);
        google.maps.event.addListener(gmPoly, 'click', function () {
          component.handlePolyClick(gmPoly);
        });
        component.myPolygons.push(gmPoly);
      });

      this.bindDropdown();
    });
  }

  getPolyPath(coords) {
    var points = [];
    coords.forEach(c => {
      points.push({
        lat: c.y,
        lng: c.x
      });
    });

    return points;
  }

  calcCentroid(polyPoints) {
    var bounds = new google.maps.LatLngBounds();
    polyPoints.forEach(p => {
      bounds.extend(p);
    });

    return bounds.getCenter();
  }

  getInfoWindowContent(poly) {
    return `<div>
              <span>
                  <div class='infoWindowItem'>
                      <label>Polygon name: </label>
                      <input id='polyName' style='display: inline-block;width: 200px;' type='text' value='${poly ? poly.Name : ''}' />
                  </div>                       
                  <p />
                  <input id='polySave' style='display: inline-block;' type='button' value='Save' /> \
                  <input id='polyDelete' style='display: inline-block;float: right;' type='button' value='Delete' />"
              </span>
            </div>`;
  }

  handlePolyClick(poly) {
    this.msgs = [];
    if (this.selectedPolygon) {
      this.selectedPolygon.setOptions({ editable: false });
    }
    this.selectedPolygon = poly;
    this.selectedPolygon.setOptions({ editable: true });
    var polyCoords = poly.getPath().getArray();
    var position = this.calcCentroid(polyCoords);
    this.infowindow.setContent(this.getInfoWindowContent(poly.Owner));
    this.infowindow.setPosition(position);
    this.infowindow.open(this.map);
    setTimeout(function () {
      document.getElementById("polyName").focus();
    }, 50);
  }

  handleSavePolygon() {
    var name = (<HTMLInputElement>document.getElementById("polyName")).value;
    this.selectedPolygon.Owner.Name = name;
    if (!this.selectedPolygon.Owner.Name) {
      alert('Please enter a name');
      return;
    }

    this.bindDropdown();

    var polyCoords = this.selectedPolygon.getPath().getArray();
    var feature = new google.maps.Data.Feature({ geometry: new google.maps.Data.Polygon([polyCoords]) });
    feature.toGeoJson(f => {
      var geoJson = JSON.stringify(f);
      var id = this.selectedPolygon.Owner.Id;
      var polyToSave = new Polygon(id, name, geoJson, null);
      if (id > 0) {
        this.api.update(polyToSave).subscribe(data => {
          this.msgs = [{ severity: 'success', summary: 'Polygon Saved', detail: '' }];
        });
      } else {
        this.api.add(polyToSave).subscribe(data => {
          this.selectedPolygon.Owner.Id = data.toString();
          this.msgs = [{ severity: 'success', summary: 'Polygon Saved', detail: '' }];
        });
      }
    });
  }

  handleDeletePolygon() {
    this.api.delete(this.selectedPolygon.Owner).subscribe(e => {
      var targetPoly = this.myPolygons.filter(p => p == this.selectedPolygon)[0];
      var index = this.myPolygons.indexOf(targetPoly);
      this.myPolygons.splice(index, 1);
      targetPoly.setMap(null);
      this.infowindow.close();

      this.bindDropdown();
    });
  }

  clearMap() {
    this.myPolygons.forEach(p => {
      p.setMap(null);
    });
    this.myPolygons.length = 0;

    if (this.infowindow) {
      this.infowindow.close();
    }

    this.msgs = [];
  }

  handleDeleteAllPolygons() {
    this.api.deleteAll().subscribe(e => {
      this.clearMap();
      this.bindDropdown();
    });
  }

  reloadAndCenter() {
    this.clearMap();
    this.loadPolygons();
    this.map.setOptions(this.options);
  }

  bindDropdown() {
    this.polys.length = 0;
    this.myPolygons.forEach(p => {
      this.polys.push(p.Owner);
    });
  }

  onSelectDropdownItem(event) {
    var targetPoly = this.myPolygons.filter(p => p.Owner == event.value)[0];
    if (!targetPoly) return;
    this.handlePolyClick(targetPoly);
    var polyCoords = targetPoly.getPath().getArray();
    var position = this.calcCentroid(polyCoords);
    this.map.panTo(position);
  }

}

export class Polygon {
  Id: string;
  Name: string;
  Data: string;
  Wkt: string;

  constructor(id: string, name: string, data: string, wkt: string) {
    this.Id = id;
    this.Name = name;
    this.Data = data;
    this.Wkt = wkt;
  }
}
