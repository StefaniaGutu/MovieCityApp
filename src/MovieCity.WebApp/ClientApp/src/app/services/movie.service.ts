import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ActorDetailsPageDto } from '../interfaces/actor-details-page-dto';
import { MovieDetailsPageDto } from '../interfaces/movie-details-page-dto';
import { MovieWithDetailsDto } from '../interfaces/movie-with-details-dto';
import { PopularAndLikedMoviesDto } from '../interfaces/popular-and-liked-movies-dto';

@Injectable({
  providedIn: 'root'
})
export class MovieService {
  url:string = environment.baseUrl;
  
  constructor(private http:HttpClient) { }

  getAllMoviesAndSeries(){
    return this.http.get<MovieWithDetailsDto[]>(this.url+'/Movie/getAllMovies');
  }

  getPopularAndLikedMovies(){
    return this.http.get<PopularAndLikedMoviesDto>(this.url+'/Home/getPopularAndLikedMovies');
  }

  getMovieDetails(movieId: string){
    return this.http.get<MovieDetailsPageDto>(this.url+'/Movie/getMovieDetails?id='+movieId);
  }

  getActorDetails(actorId: string){
    return this.http.get<ActorDetailsPageDto>(this.url+'/Actor/getActorDetails?id='+actorId);
  }

  addLike(movieId: string, isLiked: boolean){
    return this.http.post<any>(this.url+'/WatchAndLike/addLike?movieId='+movieId+'&isLiked='+isLiked, null);
  }

  addWatch(movieId: string, isAlreadyWatched: boolean){
    return this.http.post<any>(this.url+'/WatchAndLike/addWatch?movieId='+movieId+'&isAlreadyWatched='+isAlreadyWatched, null);
  }

  getWatchlist(){
    return this.http.get<MovieWithDetailsDto[]>(this.url+'/Movie/getWatchlist');
  }

  getLikedMovies(){
    return this.http.get<MovieWithDetailsDto[]>(this.url+'/Movie/getLikedMovies');
  }

  getWatchedMovies(){
    return this.http.get<MovieWithDetailsDto[]>(this.url+'/Movie/getWatchedMovies');
  }
}
