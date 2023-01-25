import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MoviesAndSeriesComponent } from './movies-and-series.component';

describe('MoviesAndSeriesComponent', () => {
  let component: MoviesAndSeriesComponent;
  let fixture: ComponentFixture<MoviesAndSeriesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MoviesAndSeriesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MoviesAndSeriesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
