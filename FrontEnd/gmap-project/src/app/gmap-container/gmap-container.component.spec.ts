import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GmapContainerComponent } from './gmap-container.component';

describe('GmapContainerComponent', () => {
  let component: GmapContainerComponent;
  let fixture: ComponentFixture<GmapContainerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GmapContainerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GmapContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
