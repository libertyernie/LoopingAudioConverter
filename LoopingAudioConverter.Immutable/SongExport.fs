namespace LoopingAudioConverter.Immutable

type WholeSongContext =
| LoopCount of LoopCount: int
| Duration of DurationSec: decimal

type SongExportType =
| PreLoopSegment
| LoopSegment
| WholeSong of Context: WholeSongContext * FadeDurationSec: decimal

type SongExport = SongExport of ExportType: SongExportType * Suffix: string
