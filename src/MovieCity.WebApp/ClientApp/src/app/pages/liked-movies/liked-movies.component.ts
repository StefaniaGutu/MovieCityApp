import { Component, OnInit } from '@angular/core';
import { MovieWithDetailsDto } from 'src/app/interfaces/movie-with-details-dto';
import { MovieService } from 'src/app/services/movie.service';

@Component({
  selector: 'app-liked-movies',
  templateUrl: './liked-movies.component.html',
  styleUrls: ['./liked-movies.component.scss']
})
export class LikedMoviesComponent implements OnInit {
  public likedMovies: MovieWithDetailsDto[] = [];
  public pageTitle: string = "Liked Movies";
  public showNoResults: boolean = false;
  
  constructor(private movieService: MovieService) { }

  ngOnInit(): void {
    this.getLikedMovies();
  }

  getLikedMovies(){
    this.movieService.getLikedMovies().subscribe((res) => {
      this.likedMovies = res;
      this.showNoResults = true;
    })
  }
}
