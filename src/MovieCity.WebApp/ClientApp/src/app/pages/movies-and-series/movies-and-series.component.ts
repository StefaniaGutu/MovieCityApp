import { Component, OnInit } from '@angular/core';
import { MovieWithDetailsDto } from 'src/app/interfaces/movie-with-details-dto';
import { MovieService } from 'src/app/services/movie.service';

@Component({
  selector: 'app-movies-and-series',
  templateUrl: './movies-and-series.component.html',
  styleUrls: ['./movies-and-series.component.scss']
})
export class MoviesAndSeriesComponent implements OnInit {
  public allMoviesAndSeries: MovieWithDetailsDto[] = [];
  public pageTitle: string = "Movies & Series";

  constructor(private movieService: MovieService) { }

  ngOnInit(): void {
    this.getAllMovies();
  }

  getAllMovies(){
    this.movieService.getAllMoviesAndSeries().subscribe((res) => {
      this.allMoviesAndSeries = res;
    })
  }
}
