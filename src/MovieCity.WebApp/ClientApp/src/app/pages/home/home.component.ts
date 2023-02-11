import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Guid } from 'guid-typescript';
import { PopularAndLikedMoviesDto } from 'src/app/interfaces/popular-and-liked-movies-dto';
import { MovieService } from 'src/app/services/movie.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  popularAndLikedMovies!: PopularAndLikedMoviesDto;

  constructor(private movieService: MovieService, public router: Router) { }

  ngOnInit(): void {
    this.getPopularAndLikedMovies();
  }

  getPopularAndLikedMovies(){
    this.movieService.getPopularAndLikedMovies().subscribe((res) => {
      this.popularAndLikedMovies = res;
    })
  }

  onClickMovie(id: Guid){
    this.router.navigate(["/movie/"+ id.toString()]);
  }
}
