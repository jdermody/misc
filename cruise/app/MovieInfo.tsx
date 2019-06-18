import React from 'react';
import { Movie, formatDate } from './data/Models';
import { QueryHilighted } from './display/QueryHilighted';
import { Range } from './display/Range';

export interface MovieInfoProps {
    movie: Movie;
    query: string;
    maxPopularity: number;
    maxVotes: number;
}

function writeText(text: string, query: string) {
    if(query && query.trim().length)
        return <QueryHilighted hilight={query} text={text}/>;
    return text;
}

export function MovieInfo(props: MovieInfoProps) {
    const { movie, maxPopularity, maxVotes } = props;
    const date = new Date(movie.releaseDate);
    return <article>
        {movie.poster
            ? <img src={movie.poster.thumbnail} />
            : <div className="noimage">
                <img src="data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==" />
                <span>No Image</span>
            </div>
        }
        <div className="content">
            <h2>{writeText(movie.title, props.query)}</h2>
            <p>{writeText(movie.overview, props.query)}</p>
            <dl>
                <dt>Release Date</dt>
                <dd>{formatDate(date)}</dd>

                <dt>Score</dt>
                <dd><Range amount={movie.score} max={10} /></dd>

                <dt>Popularity</dt>
                <dd><Range amount={movie.popularity} max={maxPopularity} /></dd>

                <dt>Votes</dt>
                <dd><Range amount={movie.votes} max={maxVotes} /></dd>
            </dl>
        </div>
    </article>;
}

