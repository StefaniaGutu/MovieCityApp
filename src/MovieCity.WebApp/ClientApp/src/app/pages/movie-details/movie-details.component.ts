import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MovieDetailsPageDto } from 'src/app/interfaces/movie-details-page-dto';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { MovieService } from 'src/app/services/movie.service';

@Component({
  selector: 'app-movie-details',
  templateUrl: './movie-details.component.html',
  styleUrls: ['./movie-details.component.scss']
})
export class MovieDetailsComponent implements OnInit {

  constructor(private movieService: MovieService, private activatedRoute: ActivatedRoute, private authenticationService:AuthenticationService) { }

  movieId: string = '';
  movieDetails!: MovieDetailsPageDto;
  isUserLoggedIn: boolean = false;
  showNewReviewSection: boolean = false;
  showReviewInput: boolean = false;

  // public newReview: NewReviewDto = 
  // {
      
  // }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe((params) => {
      this.movieId = params['id'];
    });

    this.getMovieDetails();

    this.isUserLoggedIn = this.authenticationService.isUserLoggedIn();
  }

  getMovieDetails(){
    this.movieService.getMovieDetails(this.movieId).subscribe((res) => {
      this.movieDetails = res;
    })
  }

  addLike(isLiked:boolean){
    this.movieService.addLike(this.movieId, isLiked).subscribe();
    if(isLiked && this.movieDetails.isDisliked){
      this.movieDetails.dislikesNo -= 1;
    }
    if(!isLiked && this.movieDetails.isLiked){
      this.movieDetails.likesNo -= 1;
    }

    if(isLiked){
      if(this.movieDetails.isLiked){
        this.movieDetails.likesNo = this.movieDetails.likesNo - 1;
      }
      else{
        this.movieDetails.likesNo = this.movieDetails.likesNo + 1;
      }
      this.movieDetails.isLiked = !this.movieDetails.isLiked;
      this.movieDetails.isDisliked = false;
    }
    else{
      if(this.movieDetails.isDisliked){
        this.movieDetails.dislikesNo = this.movieDetails.dislikesNo - 1;
      }
      else{
        this.movieDetails.dislikesNo = this.movieDetails.dislikesNo + 1;
      }
      this.movieDetails.isDisliked = !this.movieDetails.isDisliked;
      this.movieDetails.isLiked = false;
    }
    
  }

  addInWatched(isAlreadyWatched:boolean){
    this.movieService.addWatch(this.movieId, isAlreadyWatched).subscribe();
    if(isAlreadyWatched){
      this.movieDetails.isInWatched = !this.movieDetails.isInWatched;
    }
    else{
      this.movieDetails.isInWatchlist = !this.movieDetails.isInWatchlist;
    }
  }

  showReviewSection(){
    this.showNewReviewSection = true;
  }

  cancelReview(){
    this.showNewReviewSection = false;
    this.showReviewInput = false;
  }

  addReview(){
    
  }

  showInput(){
    this.showReviewInput = !this.showReviewInput;
  }
}
