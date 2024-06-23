extends Node2D

signal key_collected

func _Onarea_2dbody_entered(body):
	if body is Player:
		key_collected.emit()
