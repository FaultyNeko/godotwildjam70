extends Node2D

@export_enum("Rotate 90", "Slide In") var animation: String

func _on_door_open() -> void:
	$AnimationPlayer.play(animation)

func _on_door_close() -> void:
	$AnimationPlayer.play_backwards(animation)
