import React, { useContext, useEffect, useState } from 'react'
import { useParams } from "react-router-dom"
import { Box, Button, Typography } from '@material-ui/core'
import { Link } from 'react-router-dom'
import BookmarkIcon from '@material-ui/icons/Bookmark'
import BookmarkBorderIcon from '@material-ui/icons/BookmarkBorder'

import FilmCard from '../../card&tiles/FilmCard/FilmCard'
import CommentsList from '../../moleculas/CommentsList/CommentsList'
import styles from './FilmPage.module.css'
import RateArea from '../../atoms/RateArea/RateArea'
import UserContext from '../../../context'

function FilmPage() {
	let { slug } = useParams();
	const { user, setUser } = useContext(UserContext);
	const [film, setFilm] = useState(null);
	const [disableAddButton, setDisableAddButton] = useState(true);

	useEffect(() => {
		fetch(`https://localhost:5001/api/film/${slug}`)
			.then(response => response.json())
			.then(result => setFilm(result))
			.catch(_ => _);
		fetch(`https://localhost:5001/api/favorite/${slug}`)
			.then(response => response.json())
			.then(result => setDisableAddButton(result))
			.catch(_ => setDisableAddButton(true));
	}, [])

	const handleAddFavorite = () => {
		fetch(`https://localhost:5001/api/favorite/${film.id}`, {
			method: 'POST',
		});
		setDisableAddButton(true);
	}

	const handleRateChanged = (value, count) => {
		// Only one func for create, update or delete user rate,
		// so value can be newRate, newRate - oldRate, - oldRate.
		// Just add it to TotalRate.
		const newTotal = film.totalRate + value
		// The same about count.
		const newCount = film.countRate + count

		setFilm({
			...film,
			countRate: newCount,
			totalRate: newTotal,
			averageRate: newTotal / newCount
		});
	}

	return (
		<>
			<Box className={styles.container}>
				<Box className={styles.marginItem}>
					{
						film !== null
							?
							<>
								<FilmCard film={film} shouldShowControls={false} />
								<RateArea
									filmId={film.id}
									filmRate={film.averageRate}
									onAction={handleRateChanged} />
							</>
							: <Typography>Загрузка...</Typography>
					}
					{
						user.userName
							? <Box className={styles.favoriteSection}>
								<Box className={styles.favoriteTextItem}>
									<Typography variant="h6">
										Заинтересовал фильм и не хотите его потерять? Добавьте в избранное.<br />Список избранных фильмов доступен в <Link className={styles.text} to="/user">профиле</Link>
									</Typography>
								</Box>

								<Button
									className={styles.favoriteButton}
									disabled={disableAddButton}
									onClick={handleAddFavorite}
								>
									{
										disableAddButton
											?
											<div>
												<BookmarkIcon />
												Уже добавлен
											</div>
											: <div>
												<BookmarkBorderIcon />
												Добавить в Избранное
											</div>
									}
								</Button>
							</Box>
							: null

					}
				</Box>
				<Typography variant="h4" className={styles.marginItem}>
					Мнения пользователей о фильме
				</Typography>
				<CommentsList className={styles.marginItem} filmId={slug} />
			</Box>
		</>
	)
}

export default FilmPage
