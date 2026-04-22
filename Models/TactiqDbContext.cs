using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TactiqApi.Models;

public partial class TactiqDbContext : DbContext
{
    public TactiqDbContext()
    {
    }

    public TactiqDbContext(DbContextOptions<TactiqDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Equipo> Equipos { get; set; }

    public virtual DbSet<EstadisticasBase> EstadisticasBases { get; set; }

    public virtual DbSet<EstadisticasCampo> EstadisticasCampos { get; set; }

    public virtual DbSet<EstadisticasPortero> EstadisticasPorteros { get; set; }

    public virtual DbSet<Jugadore> Jugadores { get; set; }

    public virtual DbSet<JugadoresRivale> JugadoresRivales { get; set; }

    public virtual DbSet<Pabellone> Pabellones { get; set; }

    public virtual DbSet<Partido> Partidos { get; set; }

    public virtual DbSet<PorteroMapaDetalle> PorteroMapaDetalles { get; set; }

    public virtual DbSet<PorteroSituacione> PorteroSituaciones { get; set; }

    public virtual DbSet<Posicion> Posicions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SituacionJuego> SituacionJuegos { get; set; }

    public virtual DbSet<Temporada> Temporadas { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VCampoPorPartido> VCampoPorPartidos { get; set; }

    public virtual DbSet<VResultadosZaragoza> VResultadosZaragozas { get; set; }

    public virtual DbSet<VTotalesCampo> VTotalesCampos { get; set; }

    public virtual DbSet<VTotalesPortero> VTotalesPorteros { get; set; }

    public virtual DbSet<ZonasPorterium> ZonasPorteria { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=tactiq_db;Username=postgres;Password=case");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Equipo>(entity =>
        {
            entity.HasKey(e => e.IdEquipo).HasName("equipos_pkey");

            entity.ToTable("equipos");

            entity.Property(e => e.IdEquipo)
                .ValueGeneratedNever()
                .HasColumnName("id_equipo");
            entity.Property(e => e.Ciudad)
                .HasMaxLength(100)
                .HasColumnName("ciudad");
            entity.Property(e => e.IdPabellon).HasColumnName("id_pabellon");
            entity.Property(e => e.ImagenLogo).HasColumnName("imagen_logo");
            entity.Property(e => e.NombreEquipo)
                .HasMaxLength(100)
                .HasColumnName("nombre_equipo");

            entity.HasOne(d => d.IdPabellonNavigation).WithMany(p => p.Equipos)
                .HasForeignKey(d => d.IdPabellon)
                .HasConstraintName("equipos_id_pabellon_fkey");
        });

        modelBuilder.Entity<EstadisticasBase>(entity =>
        {
            entity.HasKey(e => e.IdBase).HasName("estadisticas_base_pkey");

            entity.ToTable("estadisticas_base");

            entity.Property(e => e.IdBase)
                .ValueGeneratedNever()
                .HasColumnName("id_base");
            entity.Property(e => e.IdJugador).HasColumnName("id_jugador");
            entity.Property(e => e.IdPartido).HasColumnName("id_partido");
            entity.Property(e => e.IdSituacion).HasColumnName("id_situacion");
            entity.Property(e => e.Sancion2mins1)
                .HasDefaultValue(0)
                .HasColumnName("sancion_2mins_1");
            entity.Property(e => e.Sancion2mins2)
                .HasDefaultValue(0)
                .HasColumnName("sancion_2mins_2");
            entity.Property(e => e.SancionAmarilla)
                .HasDefaultValue(0)
                .HasColumnName("sancion_amarilla");
            entity.Property(e => e.SancionAzul)
                .HasDefaultValue(0)
                .HasColumnName("sancion_azul");
            entity.Property(e => e.SancionRoja)
                .HasDefaultValue(0)
                .HasColumnName("sancion_roja");
            entity.Property(e => e.TiempoDef)
                .HasMaxLength(10)
                .HasColumnName("tiempo_def");
            entity.Property(e => e.TiempoTot)
                .HasMaxLength(10)
                .HasColumnName("tiempo_tot");
            entity.Property(e => e.ValoracionGlobal)
                .HasPrecision(5, 1)
                .HasDefaultValueSql("0")
                .HasColumnName("valoracion_global");

            entity.HasOne(d => d.IdJugadorNavigation).WithMany(p => p.EstadisticasBases)
                .HasForeignKey(d => d.IdJugador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("estadisticas_base_id_jugador_fkey");

            entity.HasOne(d => d.IdPartidoNavigation).WithMany(p => p.EstadisticasBases)
                .HasForeignKey(d => d.IdPartido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("estadisticas_base_id_partido_fkey");

            entity.HasOne(d => d.IdSituacionNavigation).WithMany(p => p.EstadisticasBases)
                .HasForeignKey(d => d.IdSituacion)
                .HasConstraintName("estadisticas_base_id_situacion_fkey");
        });

        modelBuilder.Entity<EstadisticasCampo>(entity =>
        {
            entity.HasKey(e => e.IdEstadsCampo).HasName("estadisticas_campo_pkey");

            entity.ToTable("estadisticas_campo");

            entity.HasIndex(e => e.IdBase, "estadisticas_campo_id_base_key").IsUnique();

            entity.Property(e => e.IdEstadsCampo)
                .ValueGeneratedNever()
                .HasColumnName("id_estads_campo");
            entity.Property(e => e.ContraataqueBloqueados)
                .HasDefaultValue(0)
                .HasColumnName("contraataque_bloqueados");
            entity.Property(e => e.ContraataqueFuera)
                .HasDefaultValue(0)
                .HasColumnName("contraataque_fuera");
            entity.Property(e => e.ContraataqueGoles)
                .HasDefaultValue(0)
                .HasColumnName("contraataque_goles");
            entity.Property(e => e.ContraataqueLanzamientos)
                .HasDefaultValue(0)
                .HasColumnName("contraataque_lanzamientos");
            entity.Property(e => e.ContraataqueParadas)
                .HasDefaultValue(0)
                .HasColumnName("contraataque_paradas");
            entity.Property(e => e.ContraataquePostes)
                .HasDefaultValue(0)
                .HasColumnName("contraataque_postes");
            entity.Property(e => e.ExtremoBloqueados)
                .HasDefaultValue(0)
                .HasColumnName("extremo_bloqueados");
            entity.Property(e => e.ExtremoFuera)
                .HasDefaultValue(0)
                .HasColumnName("extremo_fuera");
            entity.Property(e => e.ExtremoGoles)
                .HasDefaultValue(0)
                .HasColumnName("extremo_goles");
            entity.Property(e => e.ExtremoLanzamientos)
                .HasDefaultValue(0)
                .HasColumnName("extremo_lanzamientos");
            entity.Property(e => e.ExtremoParadas)
                .HasDefaultValue(0)
                .HasColumnName("extremo_paradas");
            entity.Property(e => e.ExtremoPostes)
                .HasDefaultValue(0)
                .HasColumnName("extremo_postes");
            entity.Property(e => e.IdBase).HasColumnName("id_base");
            entity.Property(e => e.M6Bloqueados)
                .HasDefaultValue(0)
                .HasColumnName("m6_bloqueados");
            entity.Property(e => e.M6Fuera)
                .HasDefaultValue(0)
                .HasColumnName("m6_fuera");
            entity.Property(e => e.M6Goles)
                .HasDefaultValue(0)
                .HasColumnName("m6_goles");
            entity.Property(e => e.M6Lanzamientos)
                .HasDefaultValue(0)
                .HasColumnName("m6_lanzamientos");
            entity.Property(e => e.M6Paradas)
                .HasDefaultValue(0)
                .HasColumnName("m6_paradas");
            entity.Property(e => e.M6Postes)
                .HasDefaultValue(0)
                .HasColumnName("m6_postes");
            entity.Property(e => e.M7Bloqueados)
                .HasDefaultValue(0)
                .HasColumnName("m7_bloqueados");
            entity.Property(e => e.M7Fuera)
                .HasDefaultValue(0)
                .HasColumnName("m7_fuera");
            entity.Property(e => e.M7Goles)
                .HasDefaultValue(0)
                .HasColumnName("m7_goles");
            entity.Property(e => e.M7Lanzamientos)
                .HasDefaultValue(0)
                .HasColumnName("m7_lanzamientos");
            entity.Property(e => e.M7Paradas)
                .HasDefaultValue(0)
                .HasColumnName("m7_paradas");
            entity.Property(e => e.M7Postes)
                .HasDefaultValue(0)
                .HasColumnName("m7_postes");
            entity.Property(e => e.M9Bloqueados)
                .HasDefaultValue(0)
                .HasColumnName("m9_bloqueados");
            entity.Property(e => e.M9Fuera)
                .HasDefaultValue(0)
                .HasColumnName("m9_fuera");
            entity.Property(e => e.M9Goles)
                .HasDefaultValue(0)
                .HasColumnName("m9_goles");
            entity.Property(e => e.M9Lanzamientos)
                .HasDefaultValue(0)
                .HasColumnName("m9_lanzamientos");
            entity.Property(e => e.M9Paradas)
                .HasDefaultValue(0)
                .HasColumnName("m9_paradas");
            entity.Property(e => e.M9Postes)
                .HasDefaultValue(0)
                .HasColumnName("m9_postes");
            entity.Property(e => e.TotalBloqueados)
                .HasDefaultValue(0)
                .HasColumnName("total_bloqueados");
            entity.Property(e => e.TotalFuera)
                .HasDefaultValue(0)
                .HasColumnName("total_fuera");
            entity.Property(e => e.TotalGoles)
                .HasDefaultValue(0)
                .HasColumnName("total_goles");
            entity.Property(e => e.TotalLanzamientos)
                .HasDefaultValue(0)
                .HasColumnName("total_lanzamientos");
            entity.Property(e => e.TotalParadas)
                .HasDefaultValue(0)
                .HasColumnName("total_paradas");
            entity.Property(e => e.TotalPostes)
                .HasDefaultValue(0)
                .HasColumnName("total_postes");
            entity.Property(e => e.ValoracionNegativaDobles)
                .HasDefaultValue(0)
                .HasColumnName("valoracion_negativa_dobles");
            entity.Property(e => e.ValoracionNegativaFaltaAtaque)
                .HasDefaultValue(0)
                .HasColumnName("valoracion_negativa_falta_ataque");
            entity.Property(e => e.ValoracionNegativaPasos)
                .HasDefaultValue(0)
                .HasColumnName("valoracion_negativa_pasos");
            entity.Property(e => e.ValoracionNegativaPerdida)
                .HasDefaultValue(0)
                .HasColumnName("valoracion_negativa_perdida");
            entity.Property(e => e.ValoracionPositivaAsistencia)
                .HasDefaultValue(0)
                .HasColumnName("valoracion_positiva_asistencia");
            entity.Property(e => e.ValoracionPositivaRecuperacion)
                .HasDefaultValue(0)
                .HasColumnName("valoracion_positiva_recuperacion");

            entity.HasOne(d => d.IdBaseNavigation).WithOne(p => p.EstadisticasCampo)
                .HasForeignKey<EstadisticasCampo>(d => d.IdBase)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("estadisticas_campo_id_base_fkey");
        });

        modelBuilder.Entity<EstadisticasPortero>(entity =>
        {
            entity.HasKey(e => e.IdBase).HasName("estadisticas_portero_pkey");

            entity.ToTable("estadisticas_portero");

            entity.Property(e => e.IdBase)
                .ValueGeneratedNever()
                .HasColumnName("id_base");

            entity.HasOne(d => d.IdBaseNavigation).WithOne(p => p.EstadisticasPortero)
                .HasForeignKey<EstadisticasPortero>(d => d.IdBase)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("estadisticas_portero_id_base_fkey");
        });

        modelBuilder.Entity<Jugadore>(entity =>
        {
            entity.HasKey(e => e.IdJugador).HasName("jugadores_pkey");

            entity.ToTable("jugadores");

            entity.Property(e => e.IdJugador)
                .ValueGeneratedNever()
                .HasColumnName("id_jugador");
            entity.Property(e => e.Dorsal).HasColumnName("dorsal");
            entity.Property(e => e.IdEquipo).HasColumnName("id_equipo");
            entity.Property(e => e.IdPosicion).HasColumnName("id_posicion");
            entity.Property(e => e.ImagenJugador).HasColumnName("imagen_jugador");
            entity.Property(e => e.NombreJugador)
                .HasMaxLength(100)
                .HasColumnName("nombre_jugador");

            entity.HasOne(d => d.IdEquipoNavigation).WithMany(p => p.Jugadores)
                .HasForeignKey(d => d.IdEquipo)
                .HasConstraintName("jugadores_id_equipo_fkey");

            entity.HasOne(d => d.IdPosicionNavigation).WithMany(p => p.Jugadores)
                .HasForeignKey(d => d.IdPosicion)
                .HasConstraintName("jugadores_id_posicion_fkey");
        });

        modelBuilder.Entity<JugadoresRivale>(entity =>
        {
            entity.HasKey(e => e.IdJugadorRival).HasName("jugadores_rivales_pkey");

            entity.ToTable("jugadores_rivales");

            entity.Property(e => e.IdJugadorRival)
                .ValueGeneratedNever()
                .HasColumnName("id_jugador_rival");
            entity.Property(e => e.Dorsal).HasColumnName("dorsal");
            entity.Property(e => e.IdEquipo).HasColumnName("id_equipo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Posicion)
                .HasMaxLength(50)
                .HasColumnName("posicion");

            entity.HasOne(d => d.IdEquipoNavigation).WithMany(p => p.JugadoresRivales)
                .HasForeignKey(d => d.IdEquipo)
                .HasConstraintName("jugadores_rivales_id_equipo_fkey");
        });

        modelBuilder.Entity<Pabellone>(entity =>
        {
            entity.HasKey(e => e.IdPabellon).HasName("pabellones_pkey");

            entity.ToTable("pabellones");

            entity.Property(e => e.IdPabellon).HasColumnName("id_pabellon");
            entity.Property(e => e.Ciudad)
                .HasMaxLength(100)
                .HasColumnName("ciudad");
            entity.Property(e => e.NombrePab)
                .HasMaxLength(150)
                .HasColumnName("nombre_pab");
        });

        modelBuilder.Entity<Partido>(entity =>
        {
            entity.HasKey(e => e.IdPartido).HasName("partidos_pkey");

            entity.ToTable("partidos");

            entity.Property(e => e.IdPartido)
                .ValueGeneratedNever()
                .HasColumnName("id_partido");
            entity.Property(e => e.Condicion)
                .HasMaxLength(20)
                .HasColumnName("condicion");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.GolesLocal)
                .HasDefaultValue(0)
                .HasColumnName("goles_local");
            entity.Property(e => e.GolesVisitante)
                .HasDefaultValue(0)
                .HasColumnName("goles_visitante");
            entity.Property(e => e.Hora).HasColumnName("hora");
            entity.Property(e => e.IdEquipoLocal).HasColumnName("id_equipo_local");
            entity.Property(e => e.IdEquipoVisitante).HasColumnName("id_equipo_visitante");
            entity.Property(e => e.IdPabellon).HasColumnName("id_pabellon");
            entity.Property(e => e.IdTemporada).HasColumnName("id_temporada");
            entity.Property(e => e.Jornada).HasColumnName("jornada");

            entity.HasOne(d => d.IdEquipoLocalNavigation).WithMany(p => p.PartidoIdEquipoLocalNavigations)
                .HasForeignKey(d => d.IdEquipoLocal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("partidos_id_equipo_local_fkey");

            entity.HasOne(d => d.IdEquipoVisitanteNavigation).WithMany(p => p.PartidoIdEquipoVisitanteNavigations)
                .HasForeignKey(d => d.IdEquipoVisitante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("partidos_id_equipo_visitante_fkey");

            entity.HasOne(d => d.IdPabellonNavigation).WithMany(p => p.Partidos)
                .HasForeignKey(d => d.IdPabellon)
                .HasConstraintName("partidos_id_pabellon_fkey");

            entity.HasOne(d => d.IdTemporadaNavigation).WithMany(p => p.Partidos)
                .HasForeignKey(d => d.IdTemporada)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("partidos_id_temporada_fkey");
        });

        modelBuilder.Entity<PorteroMapaDetalle>(entity =>
        {
            entity.HasKey(e => e.IdMapa).HasName("portero_mapa_detalle_pkey");

            entity.ToTable("portero_mapa_detalle");

            entity.Property(e => e.IdMapa).HasColumnName("id_mapa");
            entity.Property(e => e.IdBase).HasColumnName("id_base");
            entity.Property(e => e.IdJugadorRival).HasColumnName("id_jugador_rival");
            entity.Property(e => e.IdZona).HasColumnName("id_zona");
            entity.Property(e => e.ResultadoJugada)
                .HasMaxLength(20)
                .HasColumnName("resultado_jugada");

            entity.HasOne(d => d.IdBaseNavigation).WithMany(p => p.PorteroMapaDetalles)
                .HasForeignKey(d => d.IdBase)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("portero_mapa_detalle_id_base_fkey");

            entity.HasOne(d => d.IdJugadorRivalNavigation).WithMany(p => p.PorteroMapaDetalles)
                .HasForeignKey(d => d.IdJugadorRival)
                .HasConstraintName("portero_mapa_detalle_id_jugador_rival_fkey");

            entity.HasOne(d => d.IdZonaNavigation).WithMany(p => p.PorteroMapaDetalles)
                .HasForeignKey(d => d.IdZona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("portero_mapa_detalle_id_zona_fkey");
        });

        modelBuilder.Entity<PorteroSituacione>(entity =>
        {
            entity.HasKey(e => e.IdPorteroSit).HasName("portero_situaciones_pkey");

            entity.ToTable("portero_situaciones");

            entity.Property(e => e.IdPorteroSit).HasColumnName("id_portero_sit");
            entity.Property(e => e.GolesRecibidos)
                .HasDefaultValue(0)
                .HasColumnName("goles_recibidos");
            entity.Property(e => e.IdBase).HasColumnName("id_base");
            entity.Property(e => e.LanzamientosTotales)
                .HasDefaultValue(0)
                .HasColumnName("lanzamientos_totales");
            entity.Property(e => e.Situacion)
                .HasMaxLength(20)
                .HasColumnName("situacion");

            entity.HasOne(d => d.IdBaseNavigation).WithMany(p => p.PorteroSituaciones)
                .HasForeignKey(d => d.IdBase)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("portero_situaciones_id_base_fkey");
        });

        modelBuilder.Entity<Posicion>(entity =>
        {
            entity.HasKey(e => e.IdPosicion).HasName("posicion_pkey");

            entity.ToTable("posicion");

            entity.Property(e => e.IdPosicion).HasColumnName("id_posicion");
            entity.Property(e => e.Categoria)
                .HasMaxLength(10)
                .HasColumnName("categoria");
            entity.Property(e => e.RolEspecifico)
                .HasMaxLength(20)
                .HasColumnName("rol_especifico");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.Rol)
                .HasMaxLength(20)
                .HasColumnName("rol");
        });

        modelBuilder.Entity<SituacionJuego>(entity =>
        {
            entity.HasKey(e => e.IdSituacion).HasName("situacion_juego_pkey");

            entity.ToTable("situacion_juego");

            entity.Property(e => e.IdSituacion).HasColumnName("id_situacion");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Temporada>(entity =>
        {
            entity.HasKey(e => e.IdTemporada).HasName("temporadas_pkey");

            entity.ToTable("temporadas");

            entity.Property(e => e.IdTemporada).HasColumnName("id_temporada");
            entity.Property(e => e.Anyo).HasColumnName("anyo");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(100)
                .HasColumnName("contrasena");
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .HasColumnName("correo");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(100)
                .HasColumnName("nombre_usuario");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_id_rol_fkey");
        });

        modelBuilder.Entity<VCampoPorPartido>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_campo_por_partido");

            entity.Property(e => e.Asistencias).HasColumnName("asistencias");
            entity.Property(e => e.Condicion)
                .HasMaxLength(20)
                .HasColumnName("condicion");
            entity.Property(e => e.Dorsal).HasColumnName("dorsal");
            entity.Property(e => e.EquipoLocal)
                .HasMaxLength(100)
                .HasColumnName("equipo_local");
            entity.Property(e => e.EquipoVisitante)
                .HasMaxLength(100)
                .HasColumnName("equipo_visitante");
            entity.Property(e => e.Exclusiones).HasColumnName("exclusiones");
            entity.Property(e => e.ExtremoGoles).HasColumnName("extremo_goles");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.GolesLocal).HasColumnName("goles_local");
            entity.Property(e => e.GolesVisitante).HasColumnName("goles_visitante");
            entity.Property(e => e.IdJugador).HasColumnName("id_jugador");
            entity.Property(e => e.IdPartido).HasColumnName("id_partido");
            entity.Property(e => e.Jornada).HasColumnName("jornada");
            entity.Property(e => e.M6Goles).HasColumnName("m6_goles");
            entity.Property(e => e.M7Goles).HasColumnName("m7_goles");
            entity.Property(e => e.M9Goles).HasColumnName("m9_goles");
            entity.Property(e => e.NombreJugador)
                .HasMaxLength(100)
                .HasColumnName("nombre_jugador");
            entity.Property(e => e.Perdidas).HasColumnName("perdidas");
            entity.Property(e => e.Posicion)
                .HasMaxLength(20)
                .HasColumnName("posicion");
            entity.Property(e => e.Recuperaciones).HasColumnName("recuperaciones");
            entity.Property(e => e.TiempoTot)
                .HasMaxLength(10)
                .HasColumnName("tiempo_tot");
            entity.Property(e => e.TotalGoles).HasColumnName("total_goles");
            entity.Property(e => e.TotalLanzamientos).HasColumnName("total_lanzamientos");
            entity.Property(e => e.ValoracionGlobal)
                .HasPrecision(5, 1)
                .HasColumnName("valoracion_global");
        });

        modelBuilder.Entity<VResultadosZaragoza>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_resultados_zaragoza");

            entity.Property(e => e.Condicion)
                .HasMaxLength(20)
                .HasColumnName("condicion");
            entity.Property(e => e.EquipoLocal)
                .HasMaxLength(100)
                .HasColumnName("equipo_local");
            entity.Property(e => e.EquipoVisitante)
                .HasMaxLength(100)
                .HasColumnName("equipo_visitante");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.GolesLocal).HasColumnName("goles_local");
            entity.Property(e => e.GolesRival).HasColumnName("goles_rival");
            entity.Property(e => e.GolesVisitante).HasColumnName("goles_visitante");
            entity.Property(e => e.GolesZaragoza).HasColumnName("goles_zaragoza");
            entity.Property(e => e.IdPartido).HasColumnName("id_partido");
            entity.Property(e => e.Jornada).HasColumnName("jornada");
            entity.Property(e => e.Resultado).HasColumnName("resultado");
        });

        modelBuilder.Entity<VTotalesCampo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_totales_campo");

            entity.Property(e => e.Asistencias).HasColumnName("asistencias");
            entity.Property(e => e.Dobles).HasColumnName("dobles");
            entity.Property(e => e.Dorsal).HasColumnName("dorsal");
            entity.Property(e => e.Exclusiones2min).HasColumnName("exclusiones_2min");
            entity.Property(e => e.ExtGoles).HasColumnName("ext_goles");
            entity.Property(e => e.ExtLanz).HasColumnName("ext_lanz");
            entity.Property(e => e.FaltasAtaque).HasColumnName("faltas_ataque");
            entity.Property(e => e.IdJugador).HasColumnName("id_jugador");
            entity.Property(e => e.M6Goles).HasColumnName("m6_goles");
            entity.Property(e => e.M6Lanz).HasColumnName("m6_lanz");
            entity.Property(e => e.M7Goles).HasColumnName("m7_goles");
            entity.Property(e => e.M7Lanz).HasColumnName("m7_lanz");
            entity.Property(e => e.M9Goles).HasColumnName("m9_goles");
            entity.Property(e => e.M9Lanz).HasColumnName("m9_lanz");
            entity.Property(e => e.MinutosTotales).HasColumnName("minutos_totales");
            entity.Property(e => e.NombreJugador)
                .HasMaxLength(100)
                .HasColumnName("nombre_jugador");
            entity.Property(e => e.PartidosJugados).HasColumnName("partidos_jugados");
            entity.Property(e => e.Pasos).HasColumnName("pasos");
            entity.Property(e => e.PctEfectividad).HasColumnName("pct_efectividad");
            entity.Property(e => e.Perdidas).HasColumnName("perdidas");
            entity.Property(e => e.Posicion)
                .HasMaxLength(20)
                .HasColumnName("posicion");
            entity.Property(e => e.Recuperaciones).HasColumnName("recuperaciones");
            entity.Property(e => e.TarjetasAmarillas).HasColumnName("tarjetas_amarillas");
            entity.Property(e => e.TarjetasAzules).HasColumnName("tarjetas_azules");
            entity.Property(e => e.TarjetasRojas).HasColumnName("tarjetas_rojas");
            entity.Property(e => e.TotalGoles).HasColumnName("total_goles");
            entity.Property(e => e.TotalLanzamientos).HasColumnName("total_lanzamientos");
            entity.Property(e => e.ValoracionMedia).HasColumnName("valoracion_media");
            entity.Property(e => e.ValoracionTotal).HasColumnName("valoracion_total");
        });

        modelBuilder.Entity<VTotalesPortero>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_totales_porteros");

            entity.Property(e => e.Dorsal).HasColumnName("dorsal");
            entity.Property(e => e.Exclusiones2min).HasColumnName("exclusiones_2min");
            entity.Property(e => e.Goles7m).HasColumnName("goles_7m");
            entity.Property(e => e.GolesContra).HasColumnName("goles_contra");
            entity.Property(e => e.GolesPosicional).HasColumnName("goles_posicional");
            entity.Property(e => e.GolesRecibidos).HasColumnName("goles_recibidos");
            entity.Property(e => e.IdJugador).HasColumnName("id_jugador");
            entity.Property(e => e.Lanz7m).HasColumnName("lanz_7m");
            entity.Property(e => e.LanzContra).HasColumnName("lanz_contra");
            entity.Property(e => e.LanzPosicional).HasColumnName("lanz_posicional");
            entity.Property(e => e.LanzamientosRecibidos).HasColumnName("lanzamientos_recibidos");
            entity.Property(e => e.MinutosTotales).HasColumnName("minutos_totales");
            entity.Property(e => e.NombreJugador)
                .HasMaxLength(100)
                .HasColumnName("nombre_jugador");
            entity.Property(e => e.Paradas).HasColumnName("paradas");
            entity.Property(e => e.PartidosJugados).HasColumnName("partidos_jugados");
            entity.Property(e => e.PctParadas).HasColumnName("pct_paradas");
            entity.Property(e => e.TarjetasAmarillas).HasColumnName("tarjetas_amarillas");
            entity.Property(e => e.ValoracionTotal).HasColumnName("valoracion_total");
        });

        modelBuilder.Entity<ZonasPorterium>(entity =>
        {
            entity.HasKey(e => e.IdZona).HasName("zonas_porteria_pkey");

            entity.ToTable("zonas_porteria");

            entity.Property(e => e.IdZona)
                .ValueGeneratedNever()
                .HasColumnName("id_zona");
            entity.Property(e => e.Zona).HasColumnName("zona");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
