import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectedCanvasComponent } from '../selected-canvas.component';

describe('SelectedCanvasComponent', () => {
  let component: SelectedCanvasComponent;
  let fixture: ComponentFixture<SelectedCanvasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelectedCanvasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectedCanvasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
