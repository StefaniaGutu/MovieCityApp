import { Component, OnInit } from '@angular/core';
import { MovieWithDetailsDto } from 'src/app/interfaces/movie-with-details-dto';
import { MovieService } from 'src/app/services/movie.service';

@Component({
  selector: 'app-watched-movies',
  templateUrl: './watched-movies.component.html',
  styleUrls: ['./watched-movies.component.scss']
})
export class WatchedMoviesComponent implements OnInit {
  public watchedMovies: MovieWithDetailsDto[] = [];
  public pageTitle: string = "Watched Movies";
  public showNoResults: boolean = false;
  
  constructor(private movieService: MovieService) { }

  ngOnInit(): void {
    this.getWatchedMovies();
  }

  getWatchedMovies(){
    this.movieService.getWatchedMovies().subscribe((res) => {
      this.watchedMovies = res;
      this.showNoResults = true;
    })
  }
}
