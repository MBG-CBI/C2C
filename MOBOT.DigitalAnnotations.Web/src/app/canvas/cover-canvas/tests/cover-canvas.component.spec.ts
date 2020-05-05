import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CoverCanvasComponent } from '../cover-canvas.component';

describe('CoverCanvasComponent', () => {
  let component: CoverCanvasComponent;
  let fixture: ComponentFixture<CoverCanvasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CoverCanvasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CoverCanvasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
